﻿using System;
using System.Linq;
using System.Web.Mvc;
using ZkData;

namespace ZeroKWeb.Controllers
{
    public class ContributionsController: Controller
    {
        //
        // GET: /PayPal/
        public ActionResult Index() {
            return View("ContributionsIndex");
        }


        public ActionResult Ipn() {
            Global.Nightwatch.PayPalInterface.ImportIpnPayment(Request.Params, Request.BinaryRead(Request.ContentLength));
            return Content("");
        }


        [Auth]
        public ActionResult Redeem(string code) {
            var db = new ZkDataContext();
            if (string.IsNullOrEmpty(code)) return Content("Code is empty");
            var contrib = db.Contributions.SingleOrDefault(x => x.RedeemCode == code);
            if (contrib == null) return Content("No contribution with that code found");
            if (contrib.AccountByAccountID != null) return Content(string.Format("This contribution has been assigned to {0}, thank you.", contrib.AccountByAccountID.Name));
            var acc = db.Accounts.Find(Global.AccountID);
            contrib.AccountByAccountID = acc;
            db.SubmitAndMergeChanges();
            acc.Kudos = acc.KudosGained - acc.KudosSpent;
            db.SubmitAndMergeChanges();

            return Content(string.Format("Thank you!! {0} Kudos have been added to your account {1}", contrib.KudosValue, contrib.AccountByAccountID.Name));
        }

        public ActionResult ThankYou() {
            return View("ThankYou");
        }

        [Auth(Role = AuthRole.ZkAdmin)]
        public ActionResult AddContribution(int accountID,int kudos, string item, string currency, double gross, double grossEur, double netEur, string email, string comment, bool isSpring, DateTime date) {
            using (var db = new ZkDataContext()) {
                var acc = db.Accounts.Find(accountID);
                var contrib = new Contribution()
                              {
                                  AccountID = accountID,
                                  ManuallyAddedAccountID = Global.AccountID,
                                  KudosValue = kudos,
                                  ItemName = item,
                                  IsSpringContribution = isSpring,
                                  Comment = comment,
                                  OriginalCurrency = currency,
                                  OriginalAmount = gross,
                                  Euros = grossEur,
                                  EurosNet = netEur,
                                  Time = date,
                                  Name = acc.Name,
                                  Email = email
                              };
                db.Contributions.InsertOnSubmit(contrib);
                db.SubmitAndMergeChanges();
                acc.Kudos = acc.KudosGained - acc.KudosSpent;
                db.SubmitAndMergeChanges();
            }


            return RedirectToAction("Index");
        }

        [Auth(Role = AuthRole.ZkAdmin)]
        public ActionResult ResendEmail(int contributionID) {
            var db = new ZkDataContext();
            var contrib = db.Contributions.First(x => x.ContributionID == contributionID);
            PayPalInterface.SendEmail(contrib);
            return Content(string.Format("Email with code has been sent to {0}", contrib.Email));
        }
    }
}