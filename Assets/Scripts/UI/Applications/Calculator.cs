using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public enum CalculatorOperation
    {
        NULL,
        ADD,
        SUB,
        MUL,
        DIV,
        SQRT,
        POW
    }

    public class Calculator : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 10f; } set { } }

        public Button button0;
        public Button button1;
        public Button button2;
        public Button button3;
        public Button button4;
        public Button button5;
        public Button button6;
        public Button button7;
        public Button button8;
        public Button button9;

        public Button buttonAdd;
        public Button buttonSubtract;
        public Button buttonMultiply;
        public Button buttonDivide;

        public Button buttonSqrt;
        public Button buttonPow;

        public Button buttonEquals;

        public Button buttonClear;
        public Button buttonClearAll;

        public Text output;

        private float total;
        private float storeA;
        private float storeB;
        private CalculatorOperation op;
        private bool willClearText;

        private void Start()
        {
            GameManager.instance.RegisterApplication(this);

            button0.onClick.AddListener(delegate { UpdateStore(0f); UpdateText("0"); });
            button1.onClick.AddListener(delegate { UpdateStore(1f); UpdateText("1"); });
            button2.onClick.AddListener(delegate { UpdateStore(2f); UpdateText("2"); });
            button3.onClick.AddListener(delegate { UpdateStore(3f); UpdateText("3"); });
            button4.onClick.AddListener(delegate { UpdateStore(4f); UpdateText("4"); });
            button5.onClick.AddListener(delegate { UpdateStore(5f); UpdateText("5"); });
            button6.onClick.AddListener(delegate { UpdateStore(6f); UpdateText("6"); });
            button7.onClick.AddListener(delegate { UpdateStore(7f); UpdateText("7"); });
            button8.onClick.AddListener(delegate { UpdateStore(8f); UpdateText("8"); });
            button9.onClick.AddListener(delegate { UpdateStore(9f); UpdateText("9"); });

            buttonAdd.onClick.AddListener(delegate      { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.ADD; UpdateText(" + "); } });
            buttonSubtract.onClick.AddListener(delegate { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.SUB; UpdateText(" - "); }  });
            buttonMultiply.onClick.AddListener(delegate { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.MUL; UpdateText(" * "); }  });
            buttonDivide.onClick.AddListener(delegate   { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.DIV; UpdateText(" / "); }  });

            buttonSqrt.onClick.AddListener(delegate { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.SQRT; willClearText = true; UpdateText("root "); } });
            buttonPow.onClick.AddListener(delegate  { if (op == CalculatorOperation.NULL) { op = CalculatorOperation.POW; UpdateText(" ^ "); }  });

            buttonClear.onClick.AddListener(delegate
            {
                ResetStores();
                output.text = "";
            });

            buttonEquals.onClick.AddListener(delegate
            {
                if (op != CalculatorOperation.NULL)
                {
                    switch (op)
                    {
                        case CalculatorOperation.ADD:
                            total = storeA + storeB;
                            break;
                        case CalculatorOperation.SUB:
                            total = storeA - storeB;
                            break;
                        case CalculatorOperation.MUL:
                            total = storeA * storeB;
                            break;
                        case CalculatorOperation.DIV:
                            total = storeA / storeB;
                            break;
                        case CalculatorOperation.SQRT:
                            total = Mathf.Sqrt(storeB);
                            break;
                        case CalculatorOperation.POW:
                            total = Mathf.Pow(storeA, storeB);
                            break;
                    }

                    output.text += " = " + total.ToString();
                    willClearText = true;
                    ResetStores();
                }
            });
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
        }

        private void UpdateStore(float newDigit)
        {
            float store = op == CalculatorOperation.NULL ? storeA : storeB;
            store = float.Parse(store.ToString() + newDigit.ToString());
            if (op == CalculatorOperation.NULL)
            {
                storeA = store;
            }
            else
            {
                storeB = store;
            }
        }

        private void UpdateText(string toAppend)
        {
            if (willClearText)
            {
                output.text = string.Empty;
            }

            output.text += toAppend;
            willClearText = false;
        }

        private void ResetStores()
        {
            op = CalculatorOperation.NULL;
            storeA = 0f;
            storeB = 0f;
        }
    }
}