﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MortgageLib
{
    public class Mortgage
    {
        public double MonthlyPayment { get; private set; }
        public int Years { get; private set; }
        public DateTime StartDate { get; private set; }
        public double InterestRate { get; private set; }
        public double Principal { get; private set; }
        public double DownPayment { get; private set; }
        public double RemainingBalance { get; private set; }
        public int NumberOfPaymentsMade { get;  set; }
        public Dictionary<int, Payment> Payments { get; set; }

        public Mortgage(double principle, double interest, int years, DateTime dateTime, double down)
        {
            Principal = principle;
            InterestRate = interest / 12/100;
            Years = years*12;
            StartDate = dateTime;
            DownPayment = down;
            GetMonthlyPayment();
            Amortization();
        }

        private double GetMonthlyPayment()
        {
            
            var numerator = (Principal - DownPayment) * (InterestRate * Math.Pow((1 + InterestRate), Years));
            var denominator = (Math.Pow(1 + InterestRate, Years) - 1);
            MonthlyPayment = numerator/denominator;

            return MonthlyPayment;
        }

        public double GetPayoffAmount(Payment payment)
        {
             RemainingBalance = Principal * (1 - (Math.Pow(1 + InterestRate, -Payments.Count)) /
                                InterestRate);

            return RemainingBalance;
        }

        public void Amortization()
        {
            //Amortization Schedule Calculations//
            
            Console.WriteLine($"Monthly Payment: {MonthlyPayment:C}");
            Console.WriteLine("Amortization Schedule:");
            Console.WriteLine("Month\t\tPrincipal\tInterest\tRemaining Balance");
            double remainingBalance = Principal;
            for (int month = 1; month <= Years; month++)
            {
                double interestPayment = remainingBalance * InterestRate;
                double principalPayment = MonthlyPayment - interestPayment;
                remainingBalance -= principalPayment;

                Console.WriteLine($"{month}\t\t{principalPayment:C}\t{interestPayment:C}\t{remainingBalance:C}");
            }
            
        }

        public void MakePayment(DateTime date, double amount)
        {
            if (NumberOfPaymentsMade < Years * 12)
            {
                
                double interestPayment = RemainingBalance * InterestRate;
                double principalPayment = MonthlyPayment - interestPayment;
                RemainingBalance -= principalPayment;
                NumberOfPaymentsMade++;

                // Store payment information in the Payments dictionary
                Payments.Add(NumberOfPaymentsMade, new Payment
                {
                    PrincipalPaid = principalPayment,
                    InterestPaid = interestPayment,
                    RemainingBalance = RemainingBalance,
                    PaymentDate = date
                }) ;

                Console.WriteLine($"Payment #{NumberOfPaymentsMade}: Principal Paid = {principalPayment:C}, Interest Paid = {interestPayment:C}, Remaining Balance = {RemainingBalance:C}");
            }
            else
            {
                Console.WriteLine("Loan has already been fully paid off.");
            }


        }

        public double GetInterestPaidToDate(DateTime targetDate)
        {

            double totalInterestPaid = 0.0;
            DateTime currentDate = DateTime.Now; // Assuming the current date

            if (targetDate > currentDate)
            {
                Console.WriteLine("Target date is in the future. Please provide a valid date.");
                return totalInterestPaid;
            }

            foreach(var payment in Payments)
            {
                if (payment.Key <= NumberOfPaymentsMade && currentDate <= targetDate)
                {
                    totalInterestPaid += payment.Value.InterestPaid;
                    currentDate = currentDate.AddMonths(1);
                }
                else
                {
                    break; // Stop iterating when we reach the target date or future payments
                }
            }

            return totalInterestPaid;
        }
        
    }
}