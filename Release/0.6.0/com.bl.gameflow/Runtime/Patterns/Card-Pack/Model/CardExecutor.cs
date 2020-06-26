namespace GameFlow.Patterns
{
    public interface ICardExecutor
    {
        void Exec(Card c);
    }

    public class CardExecutor : ICardExecutor
    {
        public void Exec(Card c)
        {
            c.ExecTask().Play();
        }
    }
}