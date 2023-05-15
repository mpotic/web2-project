using Business.Dto.ArticleDto;
using Business.Dto.Auth;
using Business.Result;

namespace Business.Services
{
	public interface ISellerService
	{
		IServiceOperationResult AddArticle(NewArticleDto articleDto, JwtDto jwtDto);

		IServiceOperationResult UpdateArticle(ArticleUpdateDto articleDto, JwtDto jwtDto);

		IServiceOperationResult GetAllArticles(JwtDto jwtDto);

		IServiceOperationResult UpdateArticleProductImage(ArticleProductImageUpdateDto articleDto, JwtDto jwtDto);

		IServiceOperationResult DeleteArticle(string articleName, JwtDto jwtDto);
	}
}
