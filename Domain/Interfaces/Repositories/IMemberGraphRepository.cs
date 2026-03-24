namespace FlexFit.Domain.Interfaces.Repositories
{
    public interface IMemberGraphRepository
    {
        // Usage Tracking (Self-Healing)
        Task RecordVisitAsync(string memberId, int fitnessObjectId, string memberName = null, string objectName = null);
        Task RecordReservationAsync(string memberId, int resourceId, string memberName = null, string resourceType = null);

        // Recommendations
        Task<IEnumerable<string>> GetRecommendedObjectsAsync(string memberId);
    }
}
