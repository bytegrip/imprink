using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Imprink.WebApi.Controllers;

public record CreatePaymentIntentRequest(int Amount, string OrderId);

[ApiController]
[Route("api/stripe")]
public class StripeController : ControllerBase
{
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                PaymentMethodTypes = ["card"],
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", request.OrderId }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (StripeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                "whsec_9HyZxZ2HseAkiuRvr4MEP4ntcns9n7FA" 
            );

            Console.WriteLine($"Received Stripe event: {stripeEvent.Type}");

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                var orderId = paymentIntent?.Metadata?.GetValueOrDefault("order_id") ?? "Unknown";
                
                Console.WriteLine($"✅ Order {orderId} confirmed");
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                var orderId = paymentIntent?.Metadata?.GetValueOrDefault("order_id") ?? "Unknown";
                
                Console.WriteLine($"❌ Order {orderId} payment failed");
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            Console.WriteLine($"Webhook error: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "OK" });
    }
}