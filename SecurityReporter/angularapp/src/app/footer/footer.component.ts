import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css', '../project-search.css']
})
export class FooterComponent {
  footerSections = [
    {
      title: 'Products & Services',
      links: [
        { title: 'Value Partnerships & Consulting', url: 'https://www.siemens-healthineers.com/products-services/value-partnerships-and-consulting' },
        { title: 'Medical Imaging', url: 'https://www.siemens-healthineers.com/medical-imaging' },
        { title: 'Laboratory Diagnostics', url: 'https://www.siemens-healthineers.com/laboratory-diagnostics' },
        { title: 'Point of Care Testing', url: 'https://www.siemens-healthineers.com/point-of-care-testing' },
        { title: 'Digital Solutions & Automation', url: 'https://www.siemens-healthineers.com/digital-health-solutions' },
        { title: 'Services', url: 'https://www.siemens-healthineers.com/services' },
        { title: 'Healthcare IT', url: 'https://www.siemens-healthineers.com/infrastructure-it' },
        { title: 'Clinical Specialities & Diseases', url: 'https://www.siemens-healthineers.com/clinical-specialities' },
      ]
    },
    {
      title: 'Support & Documentation',
      links: [
        { title: 'Document Library (SDS, IFU, etc.)', url: 'https://www.siemens-healthineers.com/support-documentation/online-services/document-library' },
        { title: 'Education & Training', url: 'https://www.siemens-healthineers.com/education' },
        { title: 'PEPconnect', url: 'https://pep.siemens-info.com/' },
        { title: 'Teamplay Fleet', url: 'https://www.siemens-healthineers.com/services/customer-services/connect-platforms-and-smart-enablers/teamplay-fleet' },
        { title: 'Webshop', url: 'https://shop.healthcare.siemens.com/SIEMENS/dashboard' },
        { title: 'Online Services', url: 'https://www.siemens-healthineers.com/support-documentation/online-services' },
      ]
    },
    {
      title: 'Insights',
      links: [
        { title: 'Innovating Personalized Care', url: 'https://www.siemens-healthineers.com/insights/innovating-personalized-care' },
        { title: 'Achieving Operational Excellence', url: 'https://www.siemens-healthineers.com/insights/achieving-operational-excellence' },
        { title: 'Transforming the System of Care', url: 'https://www.siemens-healthineers.com/insights/transforming-the-system-of-care' },
        { title: 'Insights Center', url: 'https://www.siemens-healthineers.com/insights/news' },
      ]
    },
    {
      title: 'About Us',
      links: [
        { title: 'About Siemens Healthineers', url: 'https://www.siemens-healthineers.com/company' },
        { title: 'Compliance', url: 'https://www.siemens-healthineers.com/company/compliance' },
        { title: 'Conferences & Events', url: 'https://www.siemens-healthineers.com/news-and-events/conferences-events-new' },
        { title: 'Contact Us', url: 'https://www.siemens-healthineers.com/how-can-we-help-you' },
        { title: 'Investor Relations', url: 'https://www.siemens-healthineers.com/investor-relations' },
        { title: 'Job Search', url: 'https://jobs.siemens.com/healthineers/jobs?page=1' },
        { title: 'Perspectives', url: 'https://www.siemens-healthineers.com/perspectives' },
        { title: 'Press Center', url: 'https://www.siemens-healthineers.com/press' },
      ]
    },
  ];

  connectSection = [
    {
      title: 'Connect',
      links: [
        { title: 'Twitter', url: 'https://twitter.com/SiemensHealth', icon: 'bi-twitter' },
        { title: 'Facebook', url: 'https://www.facebook.com/SiemensHealthineers', icon: 'bi-facebook' },
        { title: 'Instagram', url: 'https://www.instagram.com/siemens.healthineers/', icon: 'bi-instagram' },
        { title: 'LinkedIn', url: 'https://www.linkedin.com/company/siemens-healthineers', icon: 'bi-linkedin' },
        { title: 'YouTube', url: 'https://www.youtube.com/siemenshealthineers', icon: 'bi-youtube' }
      ]
    },
  ];
}
