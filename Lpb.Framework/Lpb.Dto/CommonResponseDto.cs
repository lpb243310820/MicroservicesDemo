namespace Lpb.Dto
{
    public class CommonResponseDto
    {
        public bool Successed { get; set; }
        public string Msg { get; set; }

        public CommonResponseDto()
        {
            Successed = true;
            Msg = "³É¹¦";
        }
    }
}