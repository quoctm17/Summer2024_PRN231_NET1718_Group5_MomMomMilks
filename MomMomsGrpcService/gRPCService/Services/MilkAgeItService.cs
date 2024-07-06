using BusinessObject.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Service.Interfaces;

namespace gRPCService.Services
{
    public class MilkAgeItService : MilkAgeIt.MilkAgeItBase
    {
        private readonly IMilkAgeService _MilkAgeService;

        public MilkAgeItService(IMilkAgeService MilkAgeService)
        {
            _MilkAgeService = MilkAgeService;
        }
        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<CreateMilkAgeResponse> CreateMilkAge(CreateMilkAgeRequest request, ServerCallContext context)
        {
            if (request.Min == 0 || request.Max == 0 || string.IsNullOrEmpty(request.Unit))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var MilkAge = new MilkAge
            {
                Min = request.Min,
                Max = request.Max,
                Unit = request.Unit
            };

            var result = await _MilkAgeService.CreateMilkAge(MilkAge);
            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Create failed"));
            }

            return await Task.FromResult(new CreateMilkAgeResponse
            {
                Id = MilkAge.Id,
            });
        }

        public override async Task<ReadMilkAgeResponse> ReadMilkAge(ReadMilkAgeRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var MilkAge = await _MilkAgeService.GetSingleMilkAge(request.Id);
            if (MilkAge != null)
            {
                return await Task.FromResult(new ReadMilkAgeResponse
                {
                    Id = MilkAge.Id,
                    Min = MilkAge.Min,
                    Max = MilkAge.Max,
                    Unit = MilkAge.Unit
                });
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"No MilkAge with id {request.Id}"));
        }

        public override async Task<GetMilkAgeAllResponse> ListMilkAge(GetMilkAgeAllRequest request, ServerCallContext context)
        {
            var response = new GetMilkAgeAllResponse();
            var MilkAges = await _MilkAgeService.GetAll();

            foreach (var MilkAge in MilkAges)
            {
                response.MilkAge.Add(new ReadMilkAgeResponse
                {
                    Id = MilkAge.Id,
                    Min = MilkAge.Min,
                    Max = MilkAge.Max,
                    Unit = MilkAge.Unit
                });
            }

            return await Task.FromResult(response);
        }

        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<UpdateMilkAgeResponse> UpdateMilkAge(UpdateMilkAgeRequest request, ServerCallContext context)
        {
            if (request.Id <= 0 || request.Min == 0 || request.Max == 0 || string.IsNullOrEmpty(request.Unit))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var MilkAge = new MilkAge
            {
                Id = request.Id,
                Min = request.Min,
                Max = request.Max,
                Unit = request.Unit
            };

            var result = await _MilkAgeService.UpdateMilkAge(MilkAge);

            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Update failed"));
            }

            return await Task.FromResult(new UpdateMilkAgeResponse
            {
                Id = MilkAge.Id
            });
        }

        [Authorize(Policy = "RequireAdminRole")]
        public override async Task<DeleteMilkAgeResponse> DeleteMilkAge(DeleteMilkAgeRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var result = await _MilkAgeService.DeleteMilkAge(request.Id);

            if (!result)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Delete failed"));
            }

            return await Task.FromResult(new DeleteMilkAgeResponse
            {
                Id = request.Id
            });

        }
    }

}
