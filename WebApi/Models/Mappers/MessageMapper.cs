using Business.Models;

namespace WebAPI.Models.Mappers
{
    class MessageMapper
    {
        public static GetMessageDTO MapMessageToGetMessageDTO(MessageModel messageModel, int callerId)
        {
            if (messageModel == null) return null;
            return new GetMessageDTO(messageModel.Id, messageModel.CreationTime, messageModel.Content, UserMapper.MapUserToUserDTO(messageModel.Author, callerId));
        }
    }
}
