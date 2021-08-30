
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CieBackendAssessment.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IOptions<StripeConfig> _config;
        private readonly IStripeClient client;

        public PaymentsController(IOptions<StripeConfig> config)
        {
            _config = config;
            this.client = new StripeClient(_config.Value.SecretKey);
            StripeConfiguration.ApiKey = _config.Value.SecretKey;
        }

        /// <summary>
        /// Creates a checkout session
        /// </summary>
        /// <param name="checkoutParameters">JSON containing the redirect URLs + user e-mail</param>
        [HttpPost("create-checkout-session")]
        public async Task<ActionResult<string>> CreateCheckoutSession([FromBody]JObject checkoutParameters)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = checkoutParameters["successUrl"].ToString(),
                CancelUrl = checkoutParameters["cancelUrl"].ToString(),
                CustomerEmail = checkoutParameters["customerEMail"].ToString(),
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                      Price = _config.Value.PriceId,
                      Quantity = 1
                    },
                },
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(session.Id);
        }

        /// <summary>
        /// Creates a portal session
        /// </summary>
        /// <param name="portalParameters">JSON containing the redirect URL + user e-mail</param>
        [HttpPost("create-portal-session")]
        public async Task<ActionResult<string>> CreatePortalSession([FromBody]JObject portalParameters)
        {
            var customerSearchOptions = new CustomerListOptions { Email = portalParameters["EMail"].ToString(), Limit = 1 };
            var customerService = new CustomerService();
            StripeList<Customer> customers = customerService.List(
              customerSearchOptions
            );

            var customer = customers.Data.FirstOrDefault();

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = customer.Id,
                ReturnUrl = portalParameters["returnUrl"].ToString()
            };

            var sessionService = new Stripe.BillingPortal.SessionService(client);
            var session = await sessionService.CreateAsync(options);

            return session.Url;
        }

        [HttpGet("check-customer-subscriptions/{customerEmail}")]
        public async Task<ActionResult<bool>> CustomerHasSubscriptions(string customerEmail)
        {
            var customerSearchOptions = new CustomerListOptions { Email = customerEmail, Limit = 1 };
            var customerService = new CustomerService();
            StripeList<Customer> customers = await customerService.ListAsync(
              customerSearchOptions
            );

            var customer = customers.Data.FirstOrDefault();
            if (customer == null)
            {
                return false;
            }

            var subscriptionOptions = new SubscriptionListOptions() { Customer = customer.Id };
            var subscriptionService = new SubscriptionService();
            var customerSubscriptions = subscriptionService.List(subscriptionOptions).Data;

            if (customerSubscriptions.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
