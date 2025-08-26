using System.ComponentModel;

namespace DesafioBMG.Models.Enum
{
    public enum PaymentMethodEnum
    {
        [Description("Pix")]
        Pix = 0,

        [Description("Cartão")]
        Cartao = 1
    }
}