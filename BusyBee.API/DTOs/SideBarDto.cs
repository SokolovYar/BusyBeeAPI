namespace BusyBee.API.DTOs
{
    public class SideBarDto
    {
        public SpecialistPlateDto SpecialistPlate { get; set; } = null!;
        public List<string> CategoryMenu { get; set; } = new();
        public List<string> TagSectionSearching { get; set; } = new();
    }

    public class SpecialistPlateDto
    {
        public int SpecialistAll { get; set; }
        public int SpecialistCurrent { get; set; }
    }
}
