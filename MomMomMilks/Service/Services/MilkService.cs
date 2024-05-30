using DataTransfer;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MilkService : IMilkService
    {
        private readonly IMilkRepository _milkRepository;
        public MilkService(IMilkRepository milkRepository)
        {
            _milkRepository = milkRepository;
        }

        public async Task<List<MilkDTO>> GetAllMilk()
        {
            return await _milkRepository.GetAllMilk();
        }
    }
}
