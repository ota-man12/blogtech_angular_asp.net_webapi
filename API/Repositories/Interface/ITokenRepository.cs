using Microsoft.AspNetCore.Identity;

namespace Blog.API.Repositories.Interface
{
	public interface ITokenRepository
	{
		string CreateJWTToken(IdentityUser identityUser, List<string> roles);
	}
}

