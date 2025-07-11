using SharedKernel.Exceptions;

namespace Basket.API.Exceptions;

public class BasketNotFoundException(string userName) : NotFoundException("Cart", userName)
{
}