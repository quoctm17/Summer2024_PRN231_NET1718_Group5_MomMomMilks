using BusinessObject.Entities;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace gRPCService.Services
{
	public class BrandItService : BrandIt.BrandItBase
	{
		private readonly IBrandService _brandService;

		public BrandItService(IBrandService brandService)
        {
			_brandService = brandService;
		}

		public override async Task<CreateBrandResponse> CreateBrand(CreateBrandRequest request, ServerCallContext context)
		{
			if (request.Name == string.Empty)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

			var brand = new Brand
			{
				Name = request.Name
			};

			var result = await _brandService.CreateBrand(brand);
			if(!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Create failed"));
			}

			return await Task.FromResult(new CreateBrandResponse
			{
				Id = brand.Id,
			});
		}

		public override async Task<ReadBrandResponse> ReadBrand(ReadBrandRequest request, ServerCallContext context)
		{
			if (request.Id <= 0)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

			var brand = await _brandService.GetSingleBrand(request.Id);
			if (brand != null)
			{
				return await Task.FromResult(new ReadBrandResponse
				{
					Id = brand.Id,
					Name = brand.Name
				});
			}

			throw new RpcException(new Status(StatusCode.NotFound, $"No brand with id {request.Id}"));
		}

		public override async Task<GetAllResponse> ListBrand(GetAllRequest request, ServerCallContext context)
		{
			var response = new GetAllResponse();
			var brands = await _brandService.GetAll();

			foreach (var brand in brands)
			{
				response.Brand.Add(new ReadBrandResponse
				{
					Id = brand.Id,
					Name= brand.Name
				});
			}

			return await Task.FromResult(response);
		}

		public override async Task<UpdateBrandResponse> UpdateBrand(UpdateBrandRequest request, ServerCallContext context)
		{
			if (request.Id <= 0 || request.Name == string.Empty)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

			var brand = new Brand
			{
				Id= request.Id,
				Name = request.Name,
			};

			var result = await _brandService.UpdateBrand(brand);

			if(!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Update failed"));
			}

			return await Task.FromResult(new UpdateBrandResponse
			{
				Id = brand.Id
			});
		}

		public override async Task<DeleteBrandResponse> DeleteBrand(DeleteBrandRequest request, ServerCallContext context)
		{
			if (request.Id <= 0)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

			var result = await _brandService.DeleteBrand(request.Id);

			if(!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Delete failed"));
			}

			return await Task.FromResult(new DeleteBrandResponse
			{
				Id = request.Id
			});

		}
	}
}
