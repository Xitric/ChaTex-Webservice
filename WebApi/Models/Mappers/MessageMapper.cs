using Business.Models;

namespace WebAPI.Models.Mappers
{
    public class MessageMapper
    {
        public static GetMessageDTO MapMessageToGetMessageDTO(MessageModel messageModel, int callerId)
        {
            return new GetMessageDTO()
            {
                Id = messageModel.Id,
                Content = messageModel.Content,
                CreationTime = messageModel.CreationTime,
                Sender = UserMapper.MapUserToUserDTO(messageModel.Author, callerId)
            };
        }
    }
}
