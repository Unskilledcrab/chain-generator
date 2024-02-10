using ChainGenerator.Models;
using ChainGenerator.Services;
using Microsoft.EntityFrameworkCore;

namespace ChainGenerator.Data.DataAccessLayer
{
    public class ChainGeneratorPageModelDal
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserService userService;

        public ChainGeneratorPageModelDal(ApplicationDbContext dbContext, UserService userService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }

        public async Task<ChainGeneratorPageModel> GetChainGeneratorPageModelByIdAsync(int id)
        {
            return await dbContext.ChainGeneratorPageModel
                .Include(c => c.WidgetGeneratorModels)
                    .ThenInclude(w => w.GeneratedImageReferences)
                .Include(c => c.WidgetGeneratorModels)
                    .ThenInclude(w => w.GeneratedTextReferences)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ChainGeneratorPageModel>> GetAnonymousChainGeneratorPageModelsAsync()
        {
            return await GetChainGeneratorPageModelsByOwnerAsync(null);
        }

        public async Task<List<ChainGeneratorPageModel>> GetUserChainGeneratorPageModelsAsync()
        {
            var user = await userService.GetUserAsync();
            return await GetChainGeneratorPageModelsByOwnerAsync(user);
        }

        public async Task<List<ChainGeneratorPageModel>> GetChainGeneratorPageModelsByOwnerAsync(ApplicationUser? owner)
        {
            return await dbContext.ChainGeneratorPageModel
                .Include(c => c.WidgetGeneratorModels)
                    .ThenInclude(w => w.GeneratedImageReferences)
                .Include(c => c.WidgetGeneratorModels)
                    .ThenInclude(w => w.GeneratedTextReferences)
                .Where(c => c.Owner == owner)
                .ToListAsync();
        }

        public async Task<ChainGeneratorPageModel> CreateChainGeneratorPageModelAsync(ChainGeneratorPageModel chainGeneratorPageModel)
        {
            await UpdateOwnerAsync(chainGeneratorPageModel);
            dbContext.ChainGeneratorPageModel.Add(chainGeneratorPageModel);
            await dbContext.SaveChangesAsync();
            return chainGeneratorPageModel;
        }

        public async Task<ChainGeneratorPageModel> UpdateChainGeneratorPageModelAsync(ChainGeneratorPageModel chainGeneratorPageModel)
        {
            await UpdateOwnerAsync(chainGeneratorPageModel);
            dbContext.ChainGeneratorPageModel.Update(chainGeneratorPageModel);
            await dbContext.SaveChangesAsync();
            return chainGeneratorPageModel;
        }

        public async Task DeleteChainGeneratorPageModelAsync(int id)
        {
            var chainGeneratorPageModel = await dbContext.ChainGeneratorPageModel.FindAsync(id);
            dbContext.ChainGeneratorPageModel.Remove(chainGeneratorPageModel);
            await dbContext.SaveChangesAsync();
        }

        private async Task UpdateOwnerAsync(ChainGeneratorPageModel chainGeneratorPageModel)
        {
            var user = await userService.GetUserAsync();
            chainGeneratorPageModel.Owner = user;
        }
    }
}
