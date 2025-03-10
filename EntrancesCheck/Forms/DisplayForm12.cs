using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataAccess.Clases;
using Entity.Clases;
using InfrastructureGlobal.Clases;
using InfrastructureGlobal.Infrastructure;
using InfrastructureGlobal.Interface;

namespace EntrancesCheck.Forms
{
    public partial class DisplayForm12 : Form
    {
        private DateTime _date;
        /// <summary>
        /// ID события с которого начинается получение очередной порции событий
        /// </summary>
        private int _idEvent;
        private int _counter, _i;
       
        /// <summary>
        /// Индекс изменения размера
        /// </summary>
        private Int32 _resizeIndex;
        /// <summary>
        /// Массив счетчиков на основе которых определяется время для отображения событий о проходе
        /// </summary>
        private List<int> DevicesCounters = new List<int>();
        /// <summary>
        /// Массив элементов класса GroupBox
        /// </summary>
        private List<GroupBox> _groupBoxes = new List<GroupBox>();
        /// <summary>
        /// Работа с кадровским сервером
        /// </summary>
        private WorkWithKserv _pers = new WorkWithKserv();
        /// <summary>
        /// Работа с сервером СКД
        /// </summary>
        private WorkWithFirebird _skd = new WorkWithFirebird();
        /// <summary>
        /// Список Контроллеров которые обрабатываются на данном компьютере
        /// </summary>
        private List<Device> _devicesList = new List<Device>();

        private IMessageService _message = new MessageService();
        public DisplayForm12()
        {
            InitializeComponent();
            _date = DateTime.Now;
            SetDeviceList();
            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Tick += CheckEvent;
            _idEvent = DataTableToInt(_skd.GetFbData("select MAX(id_reg) from reg_events"));
            _counter = 0;
            timer.Start();
           

        }

        private void CheckEvent(object sender, EventArgs e)
        {
            // Проверяем не пора ли скрыть фото
            CheckPhotoTime();

            DataTable dt = _skd.GetFbData(string.Format(
               @"select t.id_reg, t.staff_id, t.configs_tree_id_resource, s.TABEL_ID " +
               "from reg_events t, staff s " +
               "where t.STAFF_ID>0 " +
                "and t.STAFF_ID = s.ID_STAFF " +
                "and t.id_reg>{0}", _idEvent));
            foreach (DataRow row in dt.Rows)
            {
                _i = 0;
                foreach (Device device in _devicesList)
                {
                    if (row["configs_tree_id_resource"].ToString() == device.DeviceId.ToString())
                    {
                        var picture = _groupBoxes[_i].Controls.OfType<PictureBox>()
                                        .Where(c => c.Name.StartsWith("pictureBox"))
                                        .ToList();
                        picture[0].Image = GetPhotoFromPers(row["TABEL_ID"].ToString());
                        picture[0].Refresh();
                        DevicesCounters[_i] = _counter;
                    }
                    _i++;
                }
                
                
                _idEvent=int.Parse(row["id_reg"].ToString());
            }

            long tick = DateTime.Now.Ticks - _date.Ticks;
            DateTime stopWatch = new DateTime();

            stopWatch = stopWatch.AddTicks(tick);
           Text = string.Format("{0:HH:mm:ss:ff} - {1}", stopWatch, _counter);
            _counter++;
        }

        private int DataTableToInt(DataTable dataTable)
        {
            return int.Parse(dataTable.Rows[0][0].ToString());
        }


        private void CheckPhotoTime()
        {
            for (int i = 0; i < DevicesCounters.Count; i++)
            {
                if (DevicesCounters[i]==_counter-10 && DevicesCounters[i]>0 || DevicesCounters[i]>_counter)
                {
                    DevicesCounters[i] = 0;
                    var picture = _groupBoxes[i].Controls.OfType<PictureBox>()
                                          .Where(c => c.Name.StartsWith("pictureBox"))
                                          .ToList();
                    picture[0].Image = null;
                    picture[0].Refresh();
                }
            }
        }
        private void SetDeviceList()
        {
            string devices;
            switch (Environment.MachineName.ToUpper())
             {
                case "MONITOR04": // 1-12 - выход
                     devices =
                     @"8647," +
                     "12281," +
                     "15915," +
                     "19549," +
                     "23183," +
                     "649036," +
                     "27561," +
                     "31195," +
                     "34829," +
                     "652670," +
                     "656304," +
                     "38463";
                    _devicesList = GetInputReadersList(devices);
                    break;

                case "MONITOR03": // 13-23 - выход
                     devices =
                      @"42097," +
                     "45731," +
                     "659938," +
                     "49365," +
                     "52999," +
                     "56633," +
                     "60267," +
                     "63901," +
                     "67535," +
                     "663572," +
                     "71169," +
                     "8647";
                    _devicesList = GetInputReadersList(devices);
                    break;

                case "MONITOR02": // 1-12 - вход
                    devices =
                   @"8647," +
                     "12281," +
                     "15915," +
                     "19549," +
                     "23183," +
                     "649036," +
                     "27561," +
                     "31195," +
                     "34829," +
                     "652670," +
                     "656304," +
                     "38463";
                    _devicesList = GetOutputReadersList(devices);
                    break;

                case "MONITOR01": // 13-23 - вход
                    devices =
                    @"42097," +
                     "45731," +
                     "659938," +
                     "49365," +
                     "52999," +
                     "56633," +
                     "60267," +
                     "63901," +
                     "67535," +
                     "663572," +
                     "71169," +
                     "8647";
                    _devicesList = GetOutputReadersList(devices);
                    break;

                 case "MONITOR05": // 13-23 - вход
                     devices =
                         @"42097," +
                         "45731," +
                         "659938," +
                         "49365," +
                         "52999," +
                         "56633," +
                         "60267," +
                         "63901," +
                         "67535," +
                         "663572," +
                         "71169," +
                         "8647";
                     _devicesList = GetOutputReadersList(devices);
                     break;

                case "BSKD04": // 1-12 - вход
                    devices =
                        @"42097," +
                        "45731," +
                        "659938," +
                        "49365," +
                        "52999," +
                        "56633," +
                        "60267," +
                        "63901," +
                        "67535," +
                        "663572," +
                        "71169," +
                        "8647";
                    _devicesList = GetInputReadersList(devices);
                    break;
                default:
                    _message.ShowError(@"Данный компьютер неизвестен");
                    break;
            }


            for (int j = 0; j < 12; j++)
            {
                DevicesCounters.Add(0);
            }
            DisplayDevicesList();
        }

        private void DisplayDevicesList()
        {

            _groupBoxes = Controls.OfType<GroupBox>()
                            .Where(c => c.Name.StartsWith("groupBox"))
                            .OrderBy(o => o.Name)
                            .ToList();
             _i = 0;
            foreach (GroupBox groupBox in _groupBoxes)
            {
                if (_i < _devicesList.Count)
                {
                    var labels = groupBox.Controls.OfType<Label>().Where(c => c.Name.StartsWith("label")).ToList();
                    if (_devicesList[_i].DeviceName != "")
                    {
                        labels[0].Text = _devicesList[_i].DeviceName;
                    }
                }
                _i++;
            }
        }

        public Bitmap GetPhotoFromPers(string tabelId)
        {
            Byte[] img;
            if (tabelId.IsNotEmptyOrNullString())
            {
                
           
            DataTable dt = _pers.GetOraDataTableReader(string.Format(

                "WITH AAA AS " +
                "(SELECT to_number(tabelid), tb_sv_id  FROM skd.staff WHERE tabelid = {0} " +
                "UNION " +
                "SELECT tn, tb_sv_id FROM newpeoples WHERE tn = '{0}') " +
                "SELECT t.photo FROM AAA " +
                "    LEFT JOIN tb_photo t USING(tb_sv_id) ", tabelId));


               
            if (dt.IsNotEmptyOrNullDataTable())
            {

                    img = (Byte[])dt.Rows[0][0];


                    MemoryStream ms = new MemoryStream(img);

                    Bitmap bitmap = new Bitmap(ms);
                    return bitmap;
            }
            }
            return null;
        }



        
        public List<Device> GetInputReadersList(string devicesList)
        {
            List<Device> readersList = new List<Device>();
            DataTable dt = _skd.GetFbData(string.Format(
                @"select t.INPUT_READER_CONFIG_ID, c.DISPLAY_NAME " +
                 "from areas_controllers t, configs_tree c " +
                 "where  t.CONFIG_TREE_ID in ({0}) " +
                    "and t.INPUT_READER_CONFIG_ID>0 " +
                    "and t.CONFIG_TREE_ID=c.ID_CONFIGS_TREE " +
                 "order by c.DISPLAY_NAME", devicesList));
            foreach (DataRow row in dt.Rows)
            {
                readersList.Add(new Device() { DeviceId = int.Parse(row["INPUT_READER_CONFIG_ID"].ToString()),
                    DeviceName = row["DISPLAY_NAME"].ToString()}
                    );
            }
            return readersList;
        }

        public List<Device> GetOutputReadersList(string devicesList)
        {
            List<Device> readersList = new List<Device>();
            DataTable dt = _skd.GetFbData(string.Format(
                @"select t.OUTPUT_READER_CONFIG_ID, c.DISPLAY_NAME " +
                 "from areas_controllers t, configs_tree c " +
                 "where  t.CONFIG_TREE_ID in ({0}) " +
                    "and t.OUTPUT_READER_CONFIG_ID>0 " +
                    "and t.CONFIG_TREE_ID=c.ID_CONFIGS_TREE " +
                 "order by c.DISPLAY_NAME", devicesList));
            foreach (DataRow row in dt.Rows)
            {
                readersList.Add(new Device()
                {
                    DeviceId = int.Parse(row["OUTPUT_READER_CONFIG_ID"].ToString()),
                    DeviceName = row["DISPLAY_NAME"].ToString()
                }
                    );
            }
            return readersList;
        }

        private void DisplayForm12_Resize(object sender, EventArgs e)
        {
           int widthInd = (Width - (groupBox01.Width * 6))/6;
          int  heightInd = (Height - (groupBox01.Height * 2) - 40)/2;
            if (widthInd<heightInd)
            {
                _resizeIndex = widthInd;
            }
            else
            {
                _resizeIndex = heightInd;
            }

            foreach (GroupBox groupBox in _groupBoxes)
            {
                groupBox.Height = groupBox.Height + _resizeIndex;
                groupBox.Width = groupBox.Width + _resizeIndex;
                //ResizeElementsGroupBox(groupBox);
            }

            groupBox02.Left = groupBox01.Left + groupBox01.Width;
            groupBox03.Left = groupBox02.Left + groupBox02.Width;
            groupBox04.Left = groupBox03.Left + groupBox03.Width;
            groupBox05.Left = groupBox04.Left + groupBox04.Width;
            groupBox06.Left = groupBox05.Left + groupBox05.Width;
            groupBox08.Left = groupBox07.Left + groupBox07.Width;
            groupBox09.Left = groupBox08.Left + groupBox08.Width;
            groupBox10.Left = groupBox09.Left + groupBox09.Width;
            groupBox11.Left = groupBox10.Left + groupBox10.Width;
            groupBox12.Left = groupBox11.Left + groupBox11.Width;

            groupBox07.Top = groupBox01.Top + groupBox02.Height;
            groupBox08.Top = groupBox02.Top + groupBox02.Height;
            groupBox09.Top = groupBox02.Top + groupBox02.Height;
            groupBox10.Top = groupBox02.Top + groupBox02.Height;
            groupBox11.Top = groupBox02.Top + groupBox02.Height;
            groupBox12.Top = groupBox02.Top + groupBox02.Height;

            pictureBox13.Top = groupBox07.Top + groupBox07.Height + 5;
            pictureBox13.Width = Width;
            pictureBox13.Height = Height - pictureBox13.Top;
        }


        private void DisplayForm12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        public List<int> GetOutputReadersList2(string devicesList)
        {
            List<int> readersList = new List<int>();
            DataTable dt = _skd.GetFbData(string.Format(
                @"select t.INPUT_READER_CONFIG_ID " +
                 "from areas_controllers t " +
                 "where  t.CONFIG_TREE_ID in ({0}) " +
                 "and t.OUTPUT_READER_CONFIG_ID>0 " +
                 "order by t.CONFIG_TREE_ID", devicesList));
            foreach (DataRow row in dt.Rows)
            {
                readersList.Add(int.Parse(row["INPUT_READER_CONFIG_ID"].ToString()));
            }
            return readersList;
        }
    }
}
