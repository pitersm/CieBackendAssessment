import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class SubscriptionService {
  baseUrl = "https://localhost:44371/api/Payments/";
  sessionId: string;

  constructor(private http: HttpClient) {}

  createCheckoutSession(
    successRoute: string,
    failRoute: string,
    eMail: string
  ): Observable<string> {
    const payload = {
      successUrl: successRoute,
      cancelUrl: failRoute,
      customerEMail: eMail,
    };
    return this.http
      .post(`${this.baseUrl}create-checkout-session`, payload)
      .pipe(
        map((response: string) => {
          const sessionId: string = response;
          return sessionId;
        })
      );
  }

  createPortalSession(returnRoute: string, eMail: string): Observable<string> {
    const payload = { returnUrl: returnRoute, EMail: eMail };
    return this.http.post(`${this.baseUrl}create-portal-session`, payload).pipe(
      map((response: string) => {
        const portalUrl: string = response;
        return portalUrl;
      })
    );
  }

  customerHasSubscriptions(eMail: string): Observable<boolean> {
    return this.http.get(`${this.baseUrl}check-customer-subscriptions/${encodeURIComponent(eMail)}`)
      .pipe(map((response: any) => {
        const hasSubscriptions: boolean = response;
        return hasSubscriptions;
      }));
  }
}
