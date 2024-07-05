using BusinessObject.Entities;
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

        public async Task AddMilkAsync(Milk milk)
        {
            await _milkRepository.AddMilkAsync(milk);
        }

        public async Task DeleteMilkAsync(int milkId)
        {
            await _milkRepository.DeleteMilkAsync(milkId);
        }

        public async Task<List<MilkDTO>> GetAllMilk()
        {
            return await _milkRepository.GetAllMilkAsync();
        }
        public async Task<MilkDTO> GetMilkById(int id)
        {
            return await _milkRepository.GetMilkByIdAsync(id);
        }

        public async Task UpdateMilkAsync(Milk milk)
        {
            await _milkRepository.UpdateMilkAsync(milk);
        }
    }
}
