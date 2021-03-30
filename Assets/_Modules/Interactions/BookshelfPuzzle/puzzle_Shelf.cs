using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle_Shelf : MonoBehaviour
{
    //list of books, the order they are inserted are also the order for the puzzle
    [SerializeField]
    public List<intr_Book> books = new List<intr_Book>();
    [SerializeField]
    public List<intr_Book> solOrder = new List<intr_Book>();
    [SerializeField]
    public List<intr_Book> pullOrder = new List<intr_Book>();
    [SerializeField]
    protected int numBooks = 3;
    [SerializeField]
    public intr_Book target_Book;
    private bool matches = true;
    private bool isSolved = false;
    public bool IsSolved { get { return isSolved; } }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (!initialDisable) {
        //    for (int i = 0; i < books.Count; i++) {
        //        books[i].GetComponent<InspectableObject>().enabled = false;
        //    }
        //    initialDisable = true;
        //}
        if (!isSolved)
        {
            if (pullOrder.Count == numBooks)
            {
                // Check equality for each book.
                for (int i = 0; i < numBooks; i++)
                {

                    if (!string.Equals(solOrder[i].gameObject.name, pullOrder[i].gameObject.name))
                    {
                        Debug.Log("false");
                        matches = false;
                    }
                }

                if (matches)
                {
                    // Puzzle was solved.
                    resetBooks();
                    target_Book.pullBook();
                    pullOrder.Clear();
                    isSolved = true;
                }
                else
                {
                    // Puzzle must reset
                    resetBooks();
                    pullOrder.Clear();
                }
            }
        }
    }

    private void resetBooks() {
        foreach (intr_Book book in pullOrder) {
            book.pushBook();
        }
    }
}
