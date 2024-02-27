namespace Narumikazuchi.CreativeGallery.Data.Tags;

[Flags]
public enum ExplicitRating
{
    None            = 0b00000000,
    Offensive       = 0x00000001,
    Suggestive      = 0x00000010,
    SexualAndNudity = 0x00000100,
    Violence        = 0x00001000,
    Gore            = 0x00010000,
    Disturbing      = 0x00100000,
    Mature          = 0x11111111,
}