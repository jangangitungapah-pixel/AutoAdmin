using AutoAdmin.Models;

namespace AutoAdmin.Services;

public class MessageGeneratorService
{
    public string GeneratePaymentReminder(
        string clientName,
        string invoiceNumber,
        decimal total,
        MessageTone tone)
    {
        var amount = total.ToString("N0");

        return tone switch
        {
            MessageTone.Friendly =>
                $"Halo {clientName}, semoga kabarnya baik ya. Aku izin mengingatkan untuk invoice {invoiceNumber} sebesar Rp {amount}. Kalau sudah sempat diproses, kabari aku ya. Terima kasih banyak.",

            MessageTone.Firm =>
                $"Halo {clientName}, kami mengingatkan bahwa invoice {invoiceNumber} sebesar Rp {amount} masih menunggu pembayaran. Mohon bantuannya untuk diproses sesuai jatuh tempo. Terima kasih.",

            MessageTone.Casual =>
                $"Halo {clientName}, izin reminder ya untuk invoice {invoiceNumber} total Rp {amount}. Kalau sudah diproses, infoin aja ya. Makasih banyak!",

            _ =>
                $"Halo {clientName}, kami ingin mengingatkan bahwa invoice {invoiceNumber} sebesar Rp {amount} masih menunggu pembayaran. Mohon konfirmasinya apabila pembayaran sudah dilakukan. Terima kasih."
        };
    }

    public string GenerateProposalFollowUp(
        string clientName,
        string proposalTitle,
        MessageTone tone)
    {
        return tone switch
        {
            MessageTone.Friendly =>
                $"Halo {clientName}, semoga harinya lancar. Aku mau follow-up proposal \"{proposalTitle}\" yang kemarin dikirim. Kalau ada bagian yang ingin didiskusikan atau disesuaikan, aku siap bantu ya.",

            MessageTone.Firm =>
                $"Halo {clientName}, kami ingin menindaklanjuti proposal \"{proposalTitle}\". Mohon informasinya apakah proposal tersebut dapat diproses ke tahap berikutnya atau perlu revisi terlebih dahulu.",

            MessageTone.Casual =>
                $"Halo {clientName}, aku follow-up proposal \"{proposalTitle}\" ya. Kalau ada feedback atau mau ngobrolin detailnya, kabarin aja.",

            _ =>
                $"Halo {clientName}, kami ingin menindaklanjuti proposal \"{proposalTitle}\" yang telah dikirim. Mohon informasinya apabila ada feedback atau pertanyaan. Terima kasih."
        };
    }
}