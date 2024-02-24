using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CP.CW1._9518
{
    public partial class Form1 : Form
    {
        CountdownEvent cde = new CountdownEvent(20);
        SemaphoreSlim gateSlim = new SemaphoreSlim(10, 10);
        SemaphoreSlim runawaySlim = new SemaphoreSlim(3, 3);
        ReaderWriterLock rwlock = new ReaderWriterLock();
        object obj = new object();
        APIService.Service1Client client = new APIService.Service1Client();

        public class Gate
        {
            public string name = "";
            public bool available = true;

            public Gate(string Name)
            {
                this.name = Name;
            }
        }


        List<Gate> gates = new List<Gate>() {
            new Gate("g1"),
            new Gate("g2"),
            new Gate("g3"),
            new Gate("g4"),
            new Gate("g5"),
            new Gate("g6"),
            new Gate("g7"),
            new Gate("g8"),
            new Gate("g9"),
            new Gate("g10"),
        };

        public Form1()
        {
            InitializeComponent();
            File.WriteAllText("output.txt", "");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            Thread thr = new Thread(() =>
            {
                getFlights();
            });
            thr.Start();

            new Thread(() => {
                cde.Wait();
                invoke(() => {
                    btnStart.Enabled = true;
                    MessageBox.Show("All done!");
                });

                cde.Reset();
            }).Start();

        }

        void getFlights()
        {
            // Read data from txt file
            string[] lines = File.ReadAllLines("input.txt");

            int initialLength = dataGridView1.Rows.Count;

            for (int i = 0; i < lines.Length; i++)
            {
                // lock access to dataGridView1 because it is a shared object
                lock (obj)
                {
                    string dataLine = lines[i];

                    string[] data = dataLine.Split(',');
                    for (int j = 0; j < data.Length; j++)
                    {
                        string value = data[j];
                        // put current date plus 3 hours
                        if (j == 2)
                        {
                            data[j] = DateTime.Now.AddHours(3).ToString();
                        }
                    }
                    invoke(() =>
                    {
                        // update dataGridView1
                        dataGridView1.Rows.Add(data);
                        // log changes to listBox1
                        listBox1.Items.Add(String.Format("Flight {0} check-in", data[0]));
                    });

                    // Handle flight
                    new Thread((object index) => {
                        handleFlight((int)index);
                    }).Start(initialLength + i);
                }
            }
        }

        void handleFlight(int index)
        {
            // we have only 10 gates available, so I am limiting concurrent threads using gates to 10
            gateSlim.Wait();

            DataGridViewRow row = dataGridView1.Rows[index];
            Gate freeGate = gates.Find(g => g.available);

            // Status Boarding
            row.Cells[2].Value = DateTime.Now.AddHours(1).ToString();
            row.Cells[3].Value = "Boarding";

            row.Cells[4].Value = freeGate.name;
            freeGate.available = false;
            
            row.Cells[3].Style.BackColor = Color.Orange;
            
            invoke(() => {
                listBox1.Items.Add(String.Format("Flight {0} Boarding", row.Cells[0].Value));
            });

            Thread.Sleep(randomTime());

            // Status Ready-to-take-off
            row.Cells[2].Value = DateTime.Now.AddMinutes(15).ToString();
            row.Cells[3].Value = "Ready-to-take-off";

            row.Cells[3].Style.BackColor = Color.Red;

            invoke(() => {
                listBox1.Items.Add(String.Format("Flight {0} Ready-to-take-off. Waiting for runaway", row.Cells[0].Value));
            });

            Thread.Sleep(randomTime());

            runawaySlim.Wait();

            // Status Takeoff
            row.Cells[2].Value = DateTime.Now.ToString();
            row.Cells[3].Value = "Takeoff";

            row.Cells[3].Style.BackColor = Color.LightGreen;

            invoke(() => {
                listBox1.Items.Add(String.Format("Flight {0} Takeoff", row.Cells[0].Value));
            });

            // lock file to avaoid concurrent editing
            rwlock.AcquireWriterLock(Timeout.Infinite);

            // Save to output.txt file
            File.AppendAllText("output.txt",
                String.Format("{0},{1},{2},{3},{4}{5}", row.Cells[0].Value.ToString(),
                row.Cells[1].Value.ToString(),
                row.Cells[2].Value.ToString(),
                row.Cells[3].Value.ToString(),
                row.Cells[4].Value.ToString(),
                "\n"));

            // unlock access to file
            rwlock.ReleaseWriterLock();

            // Send data to WCF
            string res = client.SaveToDB(
            row.Cells[0].Value.ToString(),
            row.Cells[1].Value.ToString(),
            row.Cells[2].Value.ToString(),
            row.Cells[3].Value.ToString(),
            row.Cells[4].Value.ToString());

            invoke(() => {
                listBox1.Items.Add(String.Format("Flight {0} finished.", row.Cells[0].Value));
                listBox1.Items.Add(String.Format("API: {0}", res));
            });

            freeGate.available = true;

            // signal that flight is done
            cde.Signal();
            // signal that gate is available
            gateSlim.Release();
            // signal that ranaway is available
            runawaySlim.Release();
        }

        int randomTime()
        {
            return new Random().Next(20, 40) * 100;
        }

        void invoke(Action func)
        {
            Invoke(
                new MethodInvoker(
                    () => {
                        func();
                    }
                )
            );
        }
    }
}
