import { Routes } from '@angular/router';
import { EmployeeList } from './pages/employee-list/employee-list';
import { Layout } from './pages/layout/layout';
import { Login } from './pages/login/login';
import { EmployeeForm } from './pages/employee-form/employee-form';
import { Master } from './pages/master/master';
import { Dashboard } from './pages/dashboard/dashboard';
import { Leaves } from './pages/leaves/leaves';
import { Profile } from './pages/profile/profile';

export const routes: Routes = [

    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: Login
    },
    {
        path: '',
        component: Layout,
        children: [
            {
                path: 'dashboard',
                component: Dashboard
            },
            {
                path: 'emp-list',
                component: EmployeeList
            },
            {
                path: 'leaves',
                component: Leaves
            },
            {
                path: 'emp-form',
                component: EmployeeForm
            },
            {
                path: 'master',
                component: Master
            },
            {
                path: 'profile',
                component: Profile
            }

        ]

    }
];
