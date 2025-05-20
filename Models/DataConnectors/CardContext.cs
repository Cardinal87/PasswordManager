using Microsoft.EntityFrameworkCore;
using Models.DataConnectors.EFModelsConfiguration;

namespace Models.DataConnectors
{
    class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CardModelConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CardModel> Cards { get; set; }

        public IEnumerable<CardModel> List()
        {
            return Cards.ToList();
        }

        public CardModel? GetById(int id)
        {
            return Cards.FirstOrDefault(t => t.Id == id);
        }

        public void Delete(CardModel model)
        {
            Cards.Remove(model);
        }

        public CardModel Update(CardModel model, int id)
        {
            var old_model = GetById(id);
            if (old_model == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} was not found");
            }
            old_model.Name = model.Name;
            old_model.Owner = model.Owner;
            old_model.Number = model.Number;
            old_model.IsFavourite = model.IsFavourite;
            old_model.Cvc = model.Cvc;
            old_model.Year = model.Year;
            old_model.Month = model.Month;
            return old_model;

        }

        public void Insert(CardModel model)
        {
            Cards.Add(model);
        }
    }
}
