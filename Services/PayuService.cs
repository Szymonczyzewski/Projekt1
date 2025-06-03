using System.Threading.Tasks;

namespace Projekt.Services
{
    public class PayuService
    {
        public Task<string> MakePaymentAsync(decimal amount, string description)
        {
            // Tu będzie prawdziwe wołanie do API PayU
            return Task.FromResult($"Symulowana płatność {amount} zł: {description}");
        }
    }
}
