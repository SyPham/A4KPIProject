import { INavData } from '@coreui/angular'
export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },

  {
    name: '0.System',
    url: '/system',
    icon: 'icon-puzzle',
    children: [
      {
        name: '0.1 Account Group',
        url: '/system/account-group',
        icon: 'icon-puzzle'
      },
      {
        name: '0.2 Account',
        url: '/system/account',
        icon: 'icon-puzzle'
      },

      {
        name: '0.3 Period',
        url: '/system/period',
        icon: 'icon-puzzle'
      },

      {
        name: '0.4 OC',
        url: '/system/oc',
        icon: 'icon-puzzle'
      },

      {
        name: '0.5 OC User',
        url: '/system/oc-user',
        icon: 'icon-puzzle'
      },
      {
        name: '0.6 Policy',
        url: '/system/policy',
        icon: 'icon-puzzle'
      },
      {
        name: '0.6 KPI Create',
        url: '/system/kpi-create',
        icon: 'icon-puzzle'
      },
    ]
  },
  {
    name: '1.Maintain',
    url: '/maintain',
    icon: 'icon-bell',
    children: [
      // {
      //   name: '1.1 Mailing',
      //   url: '/maintain/mailing',
      //   icon: 'icon-bell'
      // },
    ]
  },
  {
    name: '2.Transaction',
    url: '/transaction',
    icon: 'icon-cursor',
    children: [
      {
        name: '2.1 KPI Objective',
        url: '/transaction/objective',
        icon: 'icon-cursor'
      },
      {
        name: '2.2 To Do List',
        url: '/transaction/todolist',
        icon: 'icon-cursor'
      },
      {
        name: '2.3 To Do List 2',
        url: '/transaction/todolist2',
        icon: 'icon-cursor'
      },
    ]
  },
  {
    name: '3.Kanban',
    url: '/kanban',
    icon: 'icon-star',
    children: [

    ]
  },
   {
    name: '4.Report',
    url: '/report',
    icon: 'icon-calculator',
    children: [
      {
        name: 'Q1,Q3 Report 季報表',
        url: '/report/q1-q3-report',
        icon: 'icon-calculator'
      },
      {
        name: 'H1 & H2 Report',
        url: '/report/h1-h2-report',
        icon: 'icon-calculator'
      },
      {
        name: 'HQ HR Report 年中考核名單',
        url: '/report/hq-hr-report',
        icon: 'icon-calculator'
      },
      {
        name: 'GHR Report ',
        url: '/report/ghr-report',
        icon: 'icon-calculator'
      },
    ]
  }

];
