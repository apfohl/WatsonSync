using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public sealed class UserAuthenticator
{
    private readonly IUserRepository userRepository;

    public UserAuthenticator(IUserRepository userRepository) =>
        this.userRepository = userRepository;

    public Maybe<User> Authenticate(string token) =>
        userRepository.FindByToken(token);
}