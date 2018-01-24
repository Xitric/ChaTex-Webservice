using Business.Models;
using IO.Swagger.Models;

namespace WebAPI.Models.Mappers
{
    class MessageMapper
    {
        public static GetMessageDTO MapMessageToGetMessageDTO(MessageModel messageModel, int callerId)
        {
            if (messageModel == null) return null;
            return new GetMessageDTO()
            {
                Id = messageModel.Id,
                CreationTime = messageModel.CreationTime,
                Content = messageModel.Content,
                DeletionDate = messageModel.DeletionTime,
                LastEdited = messageModel.LastEdited,
                Sender = UserMapper.MapUserToUserDTO(messageModel.Author, callerId)
            };
        }
    }
}
