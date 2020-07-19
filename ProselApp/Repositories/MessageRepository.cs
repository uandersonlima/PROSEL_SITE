
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProselApp.Data;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;

namespace ProselApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ProselAppContext context;

        public MessageRepository(ProselAppContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Message message)
        {
            await context.Message.AddAsync(message);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Message message)
        {
            context.Message.Remove(message);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMultipleAsync(List<Message> msgs)
        {
            context.Message.RemoveRange(msgs);
            await context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetAllAsync(string pesquisa)
        {
            var msgs = context.Message.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(pesquisa))
            {
                msgs = msgs.Where(msg => msg.Sender.ToLower().Contains(pesquisa.Trim().ToLower()) || msg.Email.ToLower().Contains(pesquisa.Trim().ToLower()));
            }
            return await msgs.OrderByDescending(msg => msg.TimeReceived).ToListAsync();
        }

        public async Task<Message> GetByCodeAsync(int id)
        {
            return await context.Message.Where(msg => msg.Messagecode == id).FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetMultipleMsgsAsync(List<int> msgscode)
        {
            return await context.Message.Where(msg => msgscode.Contains(msg.Messagecode)).ToListAsync();
        }

        public async Task UpdateMsgAsync(Message msg)
        {
            context.Message.Update(msg);
            await context.SaveChangesAsync();
        }

        public async Task UpdateMultipleMsgsAsync(List<Message> msgs)
        {
            context.Message.UpdateRange(msgs);
            await context.SaveChangesAsync();
        }
    }
}