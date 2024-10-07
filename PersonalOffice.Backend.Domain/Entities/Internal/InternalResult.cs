namespace PersonalOffice.Backend.Domain.Entites.Internal
{
    /// <summary>
    /// ��������� ����������� ������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InternalResult<T>
    {
        /// <summary>
        /// ����������
        /// </summary>
        public T? Value { get; set; }
        /// <summary>
        /// ������ ���������
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ��������� �� ������
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}