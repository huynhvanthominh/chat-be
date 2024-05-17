using chat_be.Mappers.Abstracts;

namespace chat_be.Mappers
{
    public class Mapper : IMapper
    {
        public IUserMapper userMapper => new UserMapper();
    }
}