namespace FlightBooking.Dtos.PassengerDtos
{
    public class PassengerListItemDto
    {
        //public string PassengerId { get; set; }

        // Yolcu kolonu
        public string PassengerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        // Cinsiyet
        public string Gender { get; set; }            // Erkek / Kadın
        public string PnrNumber { get; set; }            // Erkek / Kadın

        // Tip
        public string PassengerType { get; set; }     // Yetişkin / Çocuk / Bebek

        // PNR
        // Koltuk
        public string SeatNumber { get; set; }        // 12A

        // Check-in
        public string CheckInStatus { get; set; }     // Checked-In / Not Checked

        // Ödeme
        public string PaymentStatus { get; set; }     // Paid / Pending / Failed

        // Bilet
        public string TicketStatus { get; set; }      // Issued / Not Issued

        // İletişim
        public string Phone { get; set; }

    }
}
