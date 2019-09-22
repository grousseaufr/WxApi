using System.Threading.Tasks;
using WxApi.Dtos.TrolleyTotals;

namespace WxApi.Services
{
    public interface ITrolleyCalculatorService
    {
        Task<string> Post(TrolleyTotal trolleyTotal);
        decimal GetMinTrolleyTotal(TrolleyTotal trolleyTotal);
    }
}