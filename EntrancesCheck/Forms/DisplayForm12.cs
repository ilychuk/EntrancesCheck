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
        /// <summary>
        /// Код направления события. 1 - вход, 2 - выход
        /// </summary>
        private int _direction = 0;

        private readonly DateTime _date;
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
        private readonly List<int> DevicesCounters = new List<int>();
        /// <summary>
        /// Массив элементов класса GroupBox
        /// </summary>
        private List<GroupBox> _groupBoxes = new List<GroupBox>();
        /// <summary>
        /// Работа с кадровским сервером
        /// </summary>
        private readonly WorkWithKserv _pers = new WorkWithKserv();
        /// <summary>
        /// Работа с сервером СКД
        /// </summary>
        private readonly WorkWithFirebird _skd = new WorkWithFirebird();

        private readonly WorkWithMySql _mySql = new WorkWithMySql();
        /// <summary>
        /// Список Контроллеров которые обрабатываются на данном компьютере
        /// </summary>
        private List<Device> _devicesList = new List<Device>();

        /// <summary>
        /// Класс сообщений
        /// </summary>
        private readonly IMessageService _message = new MessageService();
        public DisplayForm12()
        {
            InitializeComponent();
            _date = DateTime.Now;
            SetDeviceList();
            Timer timer = new Timer {Interval = 500};
            timer.Tick += CheckEvent;
         _idEvent = DataTableToInt(_skd.GetFbData("SELECT next VALUE FOR GEN_REG_EVENTS_ID FROM RDB$DATABASE"))-1;

         // MySQL
         // _idEvent = DataTableToInt((_mySql.GetDataTable("SELECT max(id)  FROM perco_4.event")));
          //_idEvent = 381;
            _counter = 0;
            timer.Start();
           

        }

        private void CheckEvent(object sender, EventArgs e)
        {
            // Проверяем не пора ли скрыть фото
            CheckPhotoTime();

            // MySQL
            //DataTable dt = _mySql.GetDataTable(string.Format(
            //    "SELECT e.id, e.user_id, e.device_id, e.resource_number, us.tabel_number \n " +
            //    "FROM perco_4.event e, perco_4.user_staff us \n " +
            //    "where e.user_id=us.user_id and \n " +
            //        "\t e.identifier is not null and \n " +
            //        "\t e.id > {0}",_idEvent));

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



                    // MySQL
                    //if (row["device_id"].ToString() == device.DeviceId.ToString() && row["resource_number"].ToString() == _direction.ToString())
                    //{
                    //    var picture = _groupBoxes[_i].Controls.OfType<PictureBox>()
                    //                    .Where(c => c.Name.StartsWith("pictureBox"))
                    //                    .ToList();
                    //    picture[0].Image = GetPhotoFromPers(row["TABEL_NUMBER"].ToString());
                    //    picture[0].Refresh();
                    //    DevicesCounters[_i] = _counter;
                    //}
                    _i++;
                }
                
                
                _idEvent=int.Parse(row["id_reg"].ToString());

                //MySQL
                //_idEvent = int.Parse(row["id"].ToString());
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
                    // _devicesList = GetReadersList(devices);
                     //_direction = 1;

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
                    // _devicesList = GetReadersList(devices);
                    // _direction = 1;
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
                    //_devicesList = GetReadersList(devices);
                    //_direction = 2;


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
                    //_devicesList = GetReadersList(devices);
                    //_direction = 2;


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
                     _devicesList = GetReadersList(devices);
                     _direction = 1;
                    break;

                case "BSKD04": // 1-12 - вход
                    devices =
                        @"2," +
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
                    _direction = 1;
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

                // dbkserv
                //DataTable dt = _pers.GetOraDataTableReader(string.Format(

                //    "WITH AAA AS " +
                //    "(SELECT to_number(tabelid), tb_sv_id  FROM skd.staff WHERE tabelid = {0} " +
                //    "UNION " +
                //    "SELECT tn, tb_sv_id FROM newpeoples WHERE tn = '{0}') " +
                //    "SELECT t.photo FROM AAA " +
                //    "    LEFT JOIN tb_photo t USING(tb_sv_id) ", tabelId));



                DataTable dt = _pers.GetOraDataTableReader(string.Format(

                    "select SID.PHOTO \r\n " +
                    "from hr2.staff_info_data sid \r\n "+
                    "where SID.PERSONAL_NO={0}", tabelId));

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



        public List<Device> GetReadersList(string devicesList)
        {
            List<Device> readersList = new List<Device>();

            DataTable dt = _mySql.GetDataTable(string.Format("SELECT t.id, t.name \n " +
                                                             " FROM perco_4.device t \n " +
                                                             " where t.id in ({0}) and  \n " +
                                                             " order by t.name", devicesList));
            foreach (DataRow row in dt.Rows)
            {
                readersList.Add(new Device()
                    {
                        DeviceId = int.Parse(row["id"].ToString()),
                        DeviceName = row["name"].ToString()
                    }
                );
            }
            return readersList;
        }





        public List<Device> GetInputReadersList(string devicesList)
        {
            List<Device> readersList = new List<Device>();

            //DataTable dt = _mySql.GetDataTable(string.Format("SELECT t.id, t.name \n " +
            //                                                 " FROM perco_4.device t \n " +
            //                                                 " where t.id in ({0}) \n " +
            //                                                 //"and  \n " +
            //                                                  //  " \t t.resource_number=1 \n " +
            //                                                 " order by t.name", devicesList));



            DataTable dt = _skd.GetFbData(string.Format(
                @"select t.INPUT_READER_CONFIG_ID, c.DISPLAY_NAME " +
                 "from areas_controllers t, configs_tree c " +
                 "where  t.CONFIG_TREE_ID in ({0}) " +
                    "and t.INPUT_READER_CONFIG_ID>0 " +
                    "and t.CONFIG_TREE_ID=c.ID_CONFIGS_TREE " +
                 "order by c.DISPLAY_NAME", devicesList));
            foreach (DataRow row in dt.Rows)
            {
                readersList.Add(new Device() 
                    { 
                        DeviceId = int.Parse(row["INPUT_READER_CONFIG_ID"].ToString()),
                        DeviceName = row["DISPLAY_NAME"].ToString()}
                    );

                //readersList.Add(new Device()
                //    {
                //    DeviceId = int.Parse(row["id"].ToString()),
                //    DeviceName = row["name"].ToString()
                //    }
                //);
            }
            return readersList;
        }

        public List<Device> GetOutputReadersList(string devicesList)
        {
            List<Device> readersList = new List<Device>();


            //DataTable dt = _mySql.GetDataTable(string.Format("SELECT t.id, t.name \n " +
            //                                                 " FROM perco_4.device t \n " +
            //                                                 " where t.id in ({0}) \n " +
            //                                                 //" and  \n " +
            //                                                 //" \t t.resource_number=2 \n " +
            //                                                 " order by t.name", devicesList));


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
                groupBox.Height += _resizeIndex;
                groupBox.Width += _resizeIndex;
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
