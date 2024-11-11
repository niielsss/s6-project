import React from 'react';
import { NavLink } from 'react-router-dom';
import { useLocalStorage } from '../../lib/hooks/useLocalStorage';

const links = [
    {
        id: 1,
        name: 'Home',
        url: '/',
        align: 'left'
    },
    {
        id: 2,
        name: 'Movies & Shows',
        url: '/movies-shows',
        align: 'left'
    },
    {
        id: 3,
        name: 'My List',
        url: '/list',
        align: 'right',
        loggedIn: true
    },
    {
        id: 4,
        name: 'Login',
        url: '/login',
        align: 'right',
        loggedIn: false
    },
    {
        id: 5,
        name: 'Logout',
        url: '/logout',
        align: 'right',
        loggedIn: true
    },
    {
        id: 6,
        name: 'Register',
        url: '/register',
        align: 'right',
        loggedIn: false
    },
    {
        id: 7,
        name: 'Profile',
        url: '/profile',
        align: 'right',
        loggedIn: true
    },
    // {
    //     id: 8,
    //     name: 'Admin',
    //     url: '/admin',
    //     align: 'right',
    //     loggedIn: true,
    //     admin: true
    // },
    {
        id: 9,
        name: 'Terms & Conditions',
        url: '/terms-and-conditions',
        align: 'left'
    },
    {
        id: 10,
        name: 'Privacy Policy',
        url: '/privacy-policy',
        align: 'left'
    
    }
]

function Navbar() {

    const user = localStorage.getItem('user');

    return (
        <nav className="navbar">
            <ul>
                {
                    links.map(link => {
                        if (link.align === 'left') {
                            return (
                                <li key={link.id}>
                                    <NavLink to={link.url}>{link.name}</NavLink>
                                </li>
                            )
                        }
                    })
                }
            </ul>
            <ul>
                {
                    links.map(link => {
                        if (link.align === 'right') {

                            // If the link is for logged in users only
                            if (link.loggedIn && user) {
                                return (
                                    <li key={link.id}>
                                        <NavLink to={link.url}>{link.name}</NavLink>
                                    </li>
                                )
                            }

                            // If the link is for logged out users only
                            if (!link.loggedIn && !user) {
                                return (
                                    <li key={link.id}>
                                        <NavLink to={link.url}>{link.name}</NavLink>
                                    </li>
                                )
                            }
                        }
                    })
                }
            </ul>
        </nav>
    )
}

export default Navbar;