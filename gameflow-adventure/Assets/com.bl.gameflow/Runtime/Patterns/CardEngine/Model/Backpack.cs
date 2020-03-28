using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow.Patterns
{
    public class Backpack : IEnumerable<Card>
    {
        public MonoBehaviour owner { get; private set; }

        public Backpack(MonoBehaviour owner)
        {
            this.owner = owner;
            this.executor = new CardExecutor();
        }

        public Backpack(MonoBehaviour owner, ICardExecutor executor)
        {
            this.owner = owner;
            this.executor = executor;
        }

        private List<Card> cards = new List<Card>();

        private ICardExecutor executor;

        public event System.Action<Card> onCardAdd;
        public event System.Action<Card> onCardRemove;
        public event System.Action<Card> onCardExec;

        public void Exec(Card card)
        {
            executor.Exec(card);
            onCardExec?.Invoke(card);
        }

        public void Add(Card card)
        {
            cards.Add(card);
            onCardAdd?.Invoke(card);
        }

        public void Remove(Card card)
        {
            cards.Remove(card);
            onCardRemove?.Invoke(card);
        }

        public Card Find(Predicate<Card> match)
        {
            return cards.Find(match);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return ((IEnumerable<Card>)cards).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Card>)cards).GetEnumerator();
        }
    }

}
