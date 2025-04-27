using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Hubs;
using DaberlyProjet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly AppDbContext _context;

        public ChatController(IHubContext<ChatHub> hubContext, AppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequestDTO messageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sender = await _context.Users.FindAsync(messageDto.SenderId);
            var recipient = await _context.Users.FindAsync(messageDto.RecipientId);

            if (sender == null || recipient == null)
                return BadRequest("Sender or recipient not found");

            try
            {
                var conversation = await GetOrCreateConversation(messageDto.SenderId, messageDto.RecipientId);

                var message = new Message
                {
                    SenderId = messageDto.SenderId,
                    RecipientId = messageDto.RecipientId,
                    Text = messageDto.Text,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false,
                    ConversationId = conversation.Id,
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("MessageCreated", message.Id);


                return Ok(new { MessageId = message.Id });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("conversation/{userId1}/{userId2}")]
        public async Task<IActionResult> GetConversation(int userId1, int userId2)
        {
            try
            {
                var conversation = await _context.Messages.Where(m=> (m.SenderId==userId1 && m.RecipientId==userId2) ||
                   m.SenderId == userId2 && m.RecipientId == userId1).ToListAsync();

                if (conversation == null)
                    return NotFound("Conversation not found");

                return Ok(conversation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("conversations/{userId}")]
        public async Task<IActionResult> GetUserConversations(int userId)
        {
            try
            {
                var conversations = await _context.Conversations
                    .Include(c => c.User1)
                    .Include(c => c.User2)
                    .Include(c => c.Messages)
                    .Where(c => c.User1Id == userId || c.User2Id == userId)
                    .OrderByDescending(c => c.LastUpdated)
                    .Select(c => new
                    {
                        c.Id,
                        OtherUserId = c.User1Id == userId ? c.User2Id : c.User1Id,
                        OtherUserName = c.User1Id == userId ? c.User2.FirstName : c.User1.FirstName,
                        LastMessage = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault(),
                        UnreadCount = c.Messages.Count(m => m.RecipientId == userId && !m.IsRead)
                    })
                    .ToListAsync();

                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkAsReadDto dto)
        {
            try
            {
                var messages = await _context.Messages
                    .Where(m => m.ConversationId == dto.ConversationId &&
                                m.RecipientId == dto.UserId &&
                                !m.IsRead)
                    .ToListAsync();

                foreach (var message in messages)
                {
                    message.IsRead = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new { Count = messages.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<Conversation> GetOrCreateConversation(int userId1, int userId2)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == userId1 && c.User2Id == userId2) ||
                    (c.User1Id == userId2 && c.User2Id == userId1));

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    User1Id = userId1,
                    User2Id = userId2,
                    LastUpdated = DateTime.UtcNow
                };
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }

            return conversation;
        }
    }
}