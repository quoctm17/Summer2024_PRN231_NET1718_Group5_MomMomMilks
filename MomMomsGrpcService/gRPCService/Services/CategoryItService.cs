using BusinessObject.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Service.Interfaces;

namespace gRPCService.Services
{
    public class CategoryItService : CategoryIt.CategoryItBase
    {
            private readonly ICategoryService _categoryService;

            public CategoryItService(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }

        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request, ServerCallContext context)
            {
                if (request.Name == string.Empty)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

                var category = new Category
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await _categoryService.AddCategoryAsync(category);

                return await Task.FromResult(new CreateCategoryResponse
                {
                    Id = category.Id,
                });
            }

            public override async Task<ReadCategoryResponse> ReadCategory(ReadCategoryRequest request, ServerCallContext context)
            {
                if (request.Id <= 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

                var category = await _categoryService.GetCategoryByIdAsync(request.Id);
                if (category != null)
                {
                    return await Task.FromResult(new ReadCategoryResponse
                    {
                        Id = category.Id,
                        Name = category.Name
                    });
                }

                throw new RpcException(new Status(StatusCode.NotFound, $"No Category with id {request.Id}"));
            }

            public override async Task<GetAllCategoryResponse> ListCategory(GetAllCategoryRequest request, ServerCallContext context)
            {
                var response = new GetAllCategoryResponse();
                var categories = await _categoryService.GetAllCategoriesAsync();

                foreach (var category in categories)
                {
                    response.Category.Add(new ReadCategoryResponse
                    {
                        Id = category.Id,
                        Name = category.Name
                    });
                }

                return await Task.FromResult(response);
            }
        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request, ServerCallContext context)
            {
                if (request.Id <= 0 || request.Name == string.Empty)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

                var category = new Category
                {
                    Id = request.Id,
                    Name = request.Name,
                };

                await _categoryService.UpdateCategoryAsync(category);

                return await Task.FromResult(new UpdateCategoryResponse
                {
                    Id = category.Id
                });
            }
        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<DeleteCategoryResponse> DeleteCategory(DeleteCategoryRequest request, ServerCallContext context)
            {
                if (request.Id <= 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

                await _categoryService.DeleteCategoryAsync(request.Id);

                return await Task.FromResult(new DeleteCategoryResponse
                {
                    Id = request.Id
                });

            }
        }
}
