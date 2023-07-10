namespace webapi.Enums
{
    public enum ProjectOfferStatusEnum
    {
        WAITING_FOR_OFFER_CREATION,
        OFFER_DRAFT_SENT_FOR_REVIEW,
        OFFER_SENT_FOR_SIGNATURE,
        OFFER_SIGNED_READY_FOR_INVOICING,
        INVOICED,
        INDIVIDUAL_AGREEMENT,
        RETEST_FREE_OF_CHARGE,
        OTHER,
        CANCELLED,
        PREPARED
    }
}
