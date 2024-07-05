using BusinessObject.Entities;
using Grpc.Core;
using Service.Interfaces;

namespace gRPCService.Services
{
    public class SupplierItService : SupplierIt.SupplierItBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierItService(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        public override async Task<CreateSupplierResponse> CreateSupplier(CreateSupplierRequest request, ServerCallContext context)
        {
            if (request.Name == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var supplier = new Supplier
            {
                Name = request.Name
            };

            var result = await _supplierService.CreateSupplier(supplier);
            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Create failed"));
            }

            return await Task.FromResult(new CreateSupplierResponse
            {
                Id = supplier.Id,
            });
        }

        public override async Task<ReadSupplierResponse> ReadSupplier(ReadSupplierRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var supplier = await _supplierService.GetSingleSupplier(request.Id);
            if (supplier != null)
            {
                return await Task.FromResult(new ReadSupplierResponse
                {
                    Id = supplier.Id,
                    Name = supplier.Name
                });
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"No Supplier with id {request.Id}"));
        }

        public override async Task<GetAllSupplierResponse> ListSupplier(GetAllSupplierRequest request, ServerCallContext context)
        {
            var response = new GetAllSupplierResponse();
            var suppliers = await _supplierService.GetAll();

            foreach (var supplier in suppliers)
            {
                response.Supplier.Add(new ReadSupplierResponse
                {
                    Id = supplier.Id,
                    Name = supplier.Name
                });
            }

            return await Task.FromResult(response);
        }

        public override async Task<UpdateSupplierResponse> UpdateSupplier(UpdateSupplierRequest request, ServerCallContext context)
        {
            if (request.Id <= 0 || request.Name == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var supplier = new Supplier
            {
                Id = request.Id,
                Name = request.Name,
            };

            var result = await _supplierService.UpdateSupplier(supplier);

            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Update failed"));
            }

            return await Task.FromResult(new UpdateSupplierResponse
            {
                Id = supplier.Id
            });
        }

        public override async Task<DeleteSupplierResponse> DeleteSupplier(DeleteSupplierRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var result = await _supplierService.DeleteSupplier(request.Id);

            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Delete failed"));
            }

            return await Task.FromResult(new DeleteSupplierResponse
            {
                Id = request.Id
            });

        }
    }
}
