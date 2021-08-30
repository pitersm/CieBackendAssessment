import { Component, OnDestroy} from "@angular/core";
import { SubscriptionService } from "../shared/subscription.service";
import { SubSink } from "subsink";
import { loadStripe } from "@stripe/stripe-js";
import { environment } from "../../environments/environment";
import { ActivatedRoute } from "@angular/router";
import { User } from "../../app/model/user.model";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent implements OnDestroy {
  userAction = "";
  user: User;
  subscriptionSuccessful: string;
  subscriptionEdited: string;
  subscriptionAttempted: string;
  subscriptionsLoaded = false;
  private subs = new SubSink();

  constructor(
    private subscriptionService: SubscriptionService,
    private route: ActivatedRoute
  ) {
    this.subs.add(
      this.route.queryParams.subscribe((params: any) => {
        this.subscriptionAttempted = params.subscriptionAttempted;
        this.subscriptionEdited = params.subscriptionEdited;
        this.subscriptionSuccessful = params.isSuccess;

        this.subs.add(
          this.route.data.subscribe((res: any) => {
            this.user = res.user;
            this.subs.add(this.subscriptionService
              .customerHasSubscriptions(this.user.eMail)
              .subscribe((hasSubscriptions: boolean) => {
                this.user.isSubscribed = hasSubscriptions;
                this.subscriptionsLoaded = true;
              }));
          })
        );
      })
    );
  }

  onUserAction() {
    if (this.user.isSubscribed) {
      this.manageUser();
    } else {
      this.subscribeUser();
    }
  }

  subscribeUser() {
    this.subs.add(
      this.subscriptionService
        .createCheckoutSession(
          `${window.location.origin}/home?id=${this.user.id}&subscriptionAttempted=true&isSuccess=true`,
          `${window.location.origin}/home?id=${this.user.id}&subscriptionAttempted=true&isSuccess=false`,
          this.user.eMail
        )
        .subscribe((sessionId: string) => {
          loadStripe(environment.stripePublishableKey).then((stripe) => {
            stripe.redirectToCheckout({ sessionId });
          });
        })
    );
  }

  manageUser() {
    this.subs.add(
      this.subscriptionService
        .createPortalSession(
          `${window.location.origin}/home?id=${this.user.id}&subscriptionEdited=true`,
          this.user.eMail
        )
        .subscribe((url: string) => {
          location.href = url;
        })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
