using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace HairSaloon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool initializedComponent;
        // creating a new appointmentList
        AppointmentList appointmentList = new AppointmentList();
        Customer customerObj = new Men();
        public Appointment appointment = new Appointment();
        // observableCollection of appiointments
        public ObservableCollection<Appointment> ObservableAppointment { get; private set; }
        private object selectedGridRow;

        public object SelectedGridRow
        {
            get { return selectedGridRow; }
            set { selectedGridRow = value; }
        }

        private string selectedStyle;

        // property for fetching selected style comboBox
        public string SelectedStyle
        {
            get { return selectedStyle; }
            set { selectedStyle = value; }
        }

        public List<Appointment> Appointment { get; private set; }

        private string selectedGender;
        // property for fetching selected gender
        public string SelectedGender
        {
            get { return selectedGender; }
            set { selectedGender = value; }
        }

        private string searchByType;
        // property for searching by type
        public string SearchByType
        {
            get { return searchByType; }
            set { searchByType = value; }
        }


        public MainWindow()
        {
            InitializeComponent();
            initializedComponent = true;
            SearchByType = "chkNameSearchEle";
            PopulateComboBox();
        }

        //writing to xml file
        private void WriteToXMLFile(Appointment appointment)
        {
            appointmentList.AppointmentsList.Add(appointment);
            XmlSerializer serializer = new XmlSerializer(typeof(AppointmentList));
            TextWriter textWriter = new StreamWriter("appointments.xml");
            serializer.Serialize(textWriter, appointmentList);
            textWriter.Close();
        }

        // removes appointment from xml file
        private void RemoveFromXMLFile(Appointment appointment)
        {
            appointmentList.AppointmentsList.Remove(appointment);
            XmlSerializer serializer = new XmlSerializer(typeof(AppointmentList));
            TextWriter textWriter = new StreamWriter("appointments.xml");
            serializer.Serialize(textWriter, appointmentList);
            textWriter.Close();
        }

        //read from xml file
        private void ReadFromXMLFile()
        {
            if (File.Exists("appointments.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppointmentList));
                TextReader textReader = new StreamReader("appointments.xml");
                appointmentList = (AppointmentList)serializer.Deserialize(textReader);
                
                Appointment = appointmentList.AppointmentsList;
                ObservableAppointment = new ObservableCollection<Appointment>();
                foreach (var item in Appointment) 
                    ObservableAppointment.Add(item);
                gridEle.ItemsSource = ObservableAppointment;
                textReader.Close();
            }
        }

        //button to fetch all apointments from xml and displaying it in grid
        private void btnViewAllAppointment_Click(object sender, RoutedEventArgs e)
        {
            ReadFromXMLFile();
        }

        // create slots for appointment booking 
        public List<string> CreateAppointmentSlots()
        {
            List<string> timeSlots = new List<string>();
            for (int time = 9; time < 22; time++)
            {
                if ((time > 12))
                {
                    timeSlots.Add($"{(time % 12).ToString("D2")}:00 PM");
                }
                else
                {
                    timeSlots.Add($"{time.ToString("D2")}:00 AM");
                }
            }
            return timeSlots;
        }

        //populates data for comboBox(appointment slots)
        public void PopulateComboBox()
        {
            if (File.Exists("appointments.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppointmentList));
                TextReader textReader = new StreamReader("appointments.xml");
                appointmentList = (AppointmentList)serializer.Deserialize(textReader);              
                Appointment = appointmentList.AppointmentsList;
                List<string> reservedTimeSlots = new List<string>();
                List<string> totalTimeSlots = CreateAppointmentSlots();
                foreach (var slot in Appointment)
                {
                    reservedTimeSlots.Add(slot.timeSlot.Trim());
                }

                if (reservedTimeSlots.Count == 0)
                {
                    foreach (string slotStr in totalTimeSlots)
                    {
                        // add all the slots to the comboBox
                        timeSlot.Items.Add(slotStr);
                    }
                }
                else
                {
                    // to check if slot is already taken or not, true = available, false = unavailable
                    bool availableFlag = true;
                    foreach (string totalStr in totalTimeSlots)
                    {
                        availableFlag = true;
                        foreach (string reserveStr in reservedTimeSlots)
                        {
                            // if dataStr(which was read from the file) contains slot string
                            if (reserveStr.Contains(totalStr))
                            {
                                availableFlag = false;
                            }
                        }
                        if (availableFlag)
                        {
                            // insert available slot to comboBox
                            timeSlot.Items.Add(totalStr);
                        }
                    }
                }
                textReader.Close();

            }
        }
        

        // Fetch services
        private List<string> FetchServices()
        {
            List<string> services = new List<string>();

            if (radioMenEle.IsChecked.Value)
            {
                // checkBox : Trim Beard
                if (chkTrimBeardMenEle.IsChecked.Value)
                {
                    services.Add("Trim Beard");
                }
                // checkBox : Trim Hair
                if (chkTrimHairMenEle.IsChecked.Value)
                {
                    services.Add("Trim Hair");
                }
                // checkBox : Hair Wash
                if (chkHairWashMenEle.IsChecked.Value)
                {
                    services.Add("Hair Wash");
                }
                // checkBox : Hair Color
                if (chkHairColorMenEle.IsChecked.Value)
                {
                    services.Add("Hair Color");
                }
            }

            if (radioWomenEle.IsChecked.Value)
            {
                // checkBox : Hair Dressing
                if (chkHairDressingWomenEle.IsChecked.Value)
                {
                    services.Add("Hair Dressing");
                }
                // checkBox : Hair Wash
                if (chkHairWashWomenEle.IsChecked.Value)
                {
                    services.Add("Hair Wash");
                }
                // checkBox : Hair Color
                if (chkHairColorWomenEle.IsChecked.Value)
                {
                    services.Add("Hair Color");
                }
                // checkBox : Hair Trim
                if (chkHairTrimWomenEle.IsChecked.Value)
                {
                    services.Add("Hair Trim");
                }
            }

            if (radioChildEle.IsChecked.Value)
            {
                // checkBox : Trim Hair
                if (chkTrimHairChildEle.IsChecked.Value)
                {
                    services.Add("Trim Hair");
                }
                // checkBox : Hair Wash
                if (chkHairWashChildEle.IsChecked.Value)
                {
                    services.Add("Hair Wash");
                }
                // checkBox : Hair Color
                if (chkHairColorChildEle.IsChecked.Value)
                {
                    services.Add("Hair Color");
                }
            }

            return services;
        }

        //button to save and display appointment on grid
        private void btnSaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {               
                appointment.timeSlot = timeSlot.Text;

                int age = int.Parse(txtAgeEle.Text.Trim());
                string name = txtNameEle.Text.Trim();
                string creditCard = txtCreditEle.Text.Trim();
                string email = txtEmailEle.Text.Trim();
                string phoneNumber = txtPhoneEle.Text.Trim();
                string vaccinated = IsVaccinated();

               
                // Men radio button
                if (radioMenEle.IsChecked.Value)
                {
                    appointment.Gender = "Men";
                    customerObj = new Men(name, age, phoneNumber, email, creditCard, vaccinated);
                }
                // Women radio button
                else if (radioWomenEle.IsChecked.Value)
                {
                    appointment.Gender = "Women";
                    customerObj = new Women(name, age, phoneNumber, email, creditCard, vaccinated);
                }
                // Child radio button
                else
                {
                    appointment.Gender = "Kids";
                    customerObj = new Kids(name, age, phoneNumber, email, creditCard, vaccinated);
                }
                appointment.Customer = customerObj;

                // Services
                List<string> servicesList = FetchServices();
                int noOfServicesAdded = servicesList.Count;

                appointment.Services = string.Empty;
                for(int i=0; i < noOfServicesAdded-1; i++)
                {
                    appointment.Services += $"{servicesList[i]}, ";
                }
                appointment.Services += servicesList[noOfServicesAdded-1];

                WriteToXMLFile(appointment);
                ObservableAppointment = new ObservableCollection<Appointment>();
                ObservableAppointment.Add(appointment);
                gridEle.ItemsSource = ObservableAppointment;
                timeSlot.Items.Clear();
                PopulateComboBox();
                timeSlot.SelectedIndex = 0;
                MessageBox.Show("File Saved Successfully!");
                ResetForm();
            }
        }

        // to reset the form 
        void ResetForm()
        {
            txtAgeEle.Clear();
            txtCreditEle.Clear();
            txtEmailEle.Clear();
            txtNameEle.Clear();
            txtPhoneEle.Clear();
            chkVaccineEle.IsChecked = false;
            radioMenEle.IsChecked = true;

            ResetCheckBox();
        }

        // check if customert is vaccinated or not
        private string IsVaccinated()
        {
            if (chkVaccineEle.IsChecked.Value)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        // For showing check boxes(Services) according to Gender
        private void GenderCheck(object sender, RoutedEventArgs e)
        {
            if (initializedComponent)
            {
                ResetCheckBox();
                
                appointment.TotalPayment = 0;
                lblTotalPrice.Content = appointment.TotalPayment.ToString();
                RadioButton gender = (RadioButton)sender;
                SelectedGender = gender.Name;
                if (SelectedGender == "radioMenEle")
                {
                    customerObj = new Men();
                    stackMenEle.Visibility = Visibility.Visible;
                    stackWomenEle.Visibility = Visibility.Collapsed;
                    stackChildEle.Visibility = Visibility.Collapsed;
                }
                else if (SelectedGender == "radioWomenEle")
                {
                    customerObj = new Women();
                    stackMenEle.Visibility = Visibility.Collapsed;
                    stackWomenEle.Visibility = Visibility.Visible;
                    stackChildEle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    customerObj = new Kids();
                    stackMenEle.Visibility = Visibility.Collapsed;
                    stackWomenEle.Visibility = Visibility.Collapsed;
                    stackChildEle.Visibility = Visibility.Visible;
                }
            }
        }

        // Delete Selected Row From XML File
        private void btnDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedRow = (Appointment)gridEle.SelectedItems[0]; 
            ObservableCollection<Appointment> appointmentList = (ObservableCollection<Appointment>)gridEle.ItemsSource;
            appointmentList.Remove(selectedRow);
            RemoveFromXMLFile(selectedRow);
        }

        // Check if atleast one Service is selected
        private bool IsServiceSelected()
        {
            bool serviceSelected = false;
            if (radioMenEle.IsChecked.Value)
            {
                if (chkTrimBeardMenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkTrimHairMenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairWashMenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairColorMenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
            }
            else if (radioWomenEle.IsChecked.Value)
            {
                if (chkHairDressingWomenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairWashWomenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairColorWomenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairTrimWomenEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }

            }
            else if (radioChildEle.IsChecked.Value)
            {
                if (chkTrimHairChildEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairColorChildEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }
                if (chkHairWashChildEle.IsChecked.Value)
                {
                    serviceSelected = true;
                }           
            }
            return serviceSelected;

        }

        // Check Validations 
        private bool CheckValidation()
        {
            bool isFormValid = true;

            // Name Validation
            if (txtNameEle.Text == "")
            {
                isFormValid = false;
                errorNameEle.Visibility = Visibility.Visible;
                txtNameEle.Foreground = Brushes.Red;
                txtNameEle.BorderBrush = Brushes.Red;
            }
            else
            {
                errorNameEle.Visibility = Visibility.Hidden;
                txtNameEle.BorderBrush = Brushes.LightGray;
            }

            // Age Validation
            int age = 0;
            string ageStr = txtAgeEle.Text.Trim();
            if (ageStr.Equals("") || !(int.TryParse(ageStr, out age) && (age >= 1 && age <= 80)))
            {
                isFormValid = false;
                errorAgeEle.Visibility = Visibility.Visible;
                txtAgeEle.Foreground = Brushes.Red;
                txtAgeEle.BorderBrush = Brushes.Red;
            }
            else
            {
                errorAgeEle.Visibility = Visibility.Hidden;
                txtAgeEle.BorderBrush = Brushes.LightGray;
            }

            // Credit Card Validation
            string creditCardStr = txtCreditEle.Text.Trim();
            long _;
            if (creditCardStr.Equals("") || !(creditCardStr.Length == 16 && long.TryParse(creditCardStr, out _))
)
            {
                isFormValid = false;
                errorCreditEle.Visibility = Visibility.Visible;
                txtCreditEle.Foreground = Brushes.Red;
                txtCreditEle.BorderBrush = Brushes.Red;
            }
            else
            {
                errorCreditEle.Visibility = Visibility.Hidden;
                txtCreditEle.BorderBrush = Brushes.LightGray;
            }

            // Phone Number Validation
            string phoneStr = txtPhoneEle.Text.Trim();
            if (phoneStr.Equals("") || !(phoneStr.Length == 10 && long.TryParse(phoneStr, out _))
)
            {
                isFormValid = false;
                errorPhoneEle.Visibility = Visibility.Visible;
                txtPhoneEle.Foreground = Brushes.Red;
                txtPhoneEle.BorderBrush = Brushes.Red;
            }
            else
            {
                errorPhoneEle.Visibility = Visibility.Hidden;
                txtPhoneEle.BorderBrush = Brushes.LightGray;           
            }

            // Email Validation
            string emailStr = txtEmailEle.Text.Trim();
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (emailStr.Equals("") || !regex.IsMatch(emailStr)) 
            {
                isFormValid = false;
                errorEmailEle.Visibility = Visibility.Visible;
                txtEmailEle.Foreground = Brushes.Red;
                txtEmailEle.BorderBrush = Brushes.Red;
            }
            else
            {
                errorEmailEle.Visibility = Visibility.Hidden;
                txtEmailEle.BorderBrush = Brushes.LightGray;
            }


            // Service Checked Validation
            if (!IsServiceSelected())
            {
                isFormValid = false;
                errorServicesEle.Visibility = Visibility.Visible;
            }
            else
            {
                errorServicesEle.Visibility = Visibility.Hidden;
            }

            return isFormValid;
        }

        // Got-Focus Events for name
        private void NameFocusEvent(object sender, RoutedEventArgs e)
        {
            txtNameEle.Foreground = Brushes.Black;
            txtNameEle.BorderBrush = Brushes.LightGray;
        }

        // Got-Focus Events for age
        private void AgeFocusEvent(object sender, RoutedEventArgs e)
        {
            txtAgeEle.Foreground = Brushes.Black;
            txtAgeEle.BorderBrush = Brushes.LightGray;
        }

        // Got-Focus Events for phone number
        private void PhoneFocusEvent(object sender, RoutedEventArgs e)
        {
            txtPhoneEle.Foreground = Brushes.Black;
            txtPhoneEle.BorderBrush = Brushes.LightGray;
        }

        // Got-Focus Events for credit card
        private void CreditCardFocusEvent(object sender, RoutedEventArgs e)
        {
            txtCreditEle.Foreground = Brushes.Black;
            txtCreditEle.BorderBrush = Brushes.LightGray;
        }

        // Got-Focus Events for email
        private void EmailFocusEvent(object sender, RoutedEventArgs e)
        {
            txtEmailEle.Foreground = Brushes.Black;
            txtEmailEle.BorderBrush = Brushes.LightGray;
        }

        // button to search appointment
        private void btnSearchAppointment_Click(object sender, RoutedEventArgs e)
        {
            ReadFromXMLFile();
            // search by name on selecting name radio button
            if (SearchByType == "chkNameSearchEle")
            {               
                if (txtSearchEle.Text != null)
                {
                    var result = from appointment in appointmentList.AppointmentsList
                                 where appointment.Customer.Name.ToLower().Contains(txtSearchEle.Text.ToLower().Trim())
                                 orderby appointment.Customer.Name
                                 select appointment;
                    gridEle.ItemsSource = new ObservableCollection<Appointment>(result);
                }

            }
            // search by name on selecting email radio button
            else
            {
                if (txtSearchEle.Text != null)
                {
                    var result = from appointment in appointmentList.AppointmentsList
                                 where appointment.Customer.Email.ToLower().Contains(txtSearchEle.Text.ToLower().Trim())
                                 orderby appointment.Customer.Name
                                 select appointment;

                    gridEle.ItemsSource = new ObservableCollection<Appointment>(result); 
                }
            }
        }

        //gets radio button name for search by
        private void SelectedSearchByType(object sender, RoutedEventArgs e)
        {
            if (initializedComponent)
            {
                RadioButton searchBy = (RadioButton)sender;
                SearchByType = searchBy.Name;               
            }
        }

        //to reset all check boxes to unselected
        void ResetCheckBox()
        {
            chkTrimBeardMenEle.IsChecked = false;
            chkHairColorMenEle.IsChecked = false;
            chkHairWashMenEle.IsChecked = false;
            chkTrimHairMenEle.IsChecked = false;

            chkHairWashWomenEle.IsChecked = false;
            chkHairTrimWomenEle.IsChecked = false;
            chkHairDressingWomenEle.IsChecked = false;
            chkHairColorWomenEle.IsChecked = false;

            chkHairColorChildEle.IsChecked = false;
            chkTrimHairChildEle.IsChecked = false;
            chkHairWashChildEle.IsChecked = false;
        }

        //for calculating total price for the services on checked (adding when the check boxes are selected)
        private void StyleChecked(object sender, RoutedEventArgs e)
        {          
            if (initializedComponent)
            {             
                CheckBox style = (CheckBox)sender;

                SelectedStyle = (string)style.Content;
               
                if ((customerObj.GetType().Name).ToString() == "Men")
                {
                    appointment.TotalPayment += ((Men)customerObj).Styles[SelectedStyle];
                }
                else if ((customerObj.GetType().Name).ToString() == "Women")
                {
                    appointment.TotalPayment += ((Women)customerObj).Styles[SelectedStyle];
                }
                else
                {
                    appointment.TotalPayment += ((Kids)customerObj).Styles[SelectedStyle];
                }
                lblTotalPrice.Content = appointment.TotalPayment.ToString();
            }
        }

        //for calculating total price for the services on checked (subtracting when unchecking checked boxes)
        private void StyleUnChecked(object sender, RoutedEventArgs e)
        {
            if (initializedComponent)
            {
                CheckBox style = (CheckBox)sender;

                SelectedStyle = (string)style.Content;
                
                if ((customerObj.GetType().Name).ToString() == "Men")
                {
                    appointment.TotalPayment -= ((Men)customerObj).Styles[SelectedStyle];
                }
                else if ((customerObj.GetType().Name).ToString() == "Women")
                {
                    appointment.TotalPayment -= ((Women)customerObj).Styles[SelectedStyle];
                }
                else
                {
                    appointment.TotalPayment -= ((Kids)customerObj).Styles[SelectedStyle];
                }
                lblTotalPrice.Content = appointment.TotalPayment.ToString();
            }
        }
    }
}
