using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository msgRepos;

        public MessageService(IMessageRepository msgRepos)
        {
            this.msgRepos = msgRepos;
        }

        public async Task AddAsync(Message message)
        {
            message.TimeReceived = DateTime.Now;
            await msgRepos.AddAsync(message);
        }

        public async Task DeleteAsync(Message message)
        {
            await msgRepos.DeleteAsync(message);
        }

        public async Task DeleteMultipleAsync(List<Message> msgs)
        {
            await msgRepos.DeleteMultipleAsync(msgs);
        }

        public async Task<List<Message>> GetAllAsync(string pesquisa)
        {
            return await msgRepos.GetAllAsync(pesquisa);
        }

        public async Task<Message> GetByCodeAsync(int id)
        {
            return await msgRepos.GetByCodeAsync(id);
        }

        public async Task<List<Message>> GetMultipleMsgsAsync(List<int> msgscode)
        {
            return await msgRepos.GetMultipleMsgsAsync(msgscode);
        }

        public async Task UpdateMsgAsync(Message msg)
        {
            await msgRepos.UpdateMsgAsync(msg);
        }

        public async Task UpdateMultipleMsgsAsync(List<Message> msgs)
        {
            await msgRepos.UpdateMultipleMsgsAsync(msgs);
        }
    }
}