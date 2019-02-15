using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using TeknikServis.Models.Enums;

namespace TeknikServis.BLL.Services
{
    public interface IMessageService
    {
        MessageStates MessageState { get; }

        Task SendAsync(IdentityMessage message, params string[] contacts);
        void Send(IdentityMessage message, params string[] contacts);
    }
}
