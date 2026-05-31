'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { cn } from '@/lib/utils';
import {
  LayoutDashboard,
  Briefcase,
  Calendar,
  User,
  Target,
  LogOut,
  ChevronLeft,
  Menu,
} from 'lucide-react';
import { useAuth } from '@/contexts/auth-context';
import { Button } from '@/components/ui/button';
import { ThemeToggle } from './theme-toggle';
import { useState } from 'react';

const navItems = [
  {
    label: 'Dashboard',
    href: '/dashboard',
    icon: LayoutDashboard,
  },
  {
    label: 'Applications',
    href: '/applications',
    icon: Briefcase,
  },
  {
    label: 'Calendar',
    href: '/calendar',
    icon: Calendar,
  },
  {
    label: 'Match Job',
    href: '/match',
    icon: Target,
  },
  {
    label: 'Profile',
    href: '/profile',
    icon: User,
  },
];

export function Sidebar() {
  const pathname = usePathname();
  const { user, logout } = useAuth();
  const [collapsed, setCollapsed] = useState(false);

  const handleLogout = async () => {
    await logout();
    window.location.href = '/login';
  };

  return (
    <>
      {/* Mobile menu button */}
      <Button
        variant="ghost"
        size="icon"
        className="fixed top-4 left-4 z-50 md:hidden"
        onClick={() => setCollapsed(!collapsed)}
      >
        <Menu className="h-5 w-5" />
      </Button>

      {/* Sidebar */}
      <aside
        className={cn(
          'fixed inset-y-0 left-0 z-40 flex flex-col bg-sidebar border-r border-sidebar-border transition-all duration-300',
          collapsed ? 'w-16' : 'w-64',
          'max-md:translate-x-[-100%] max-md:data-[open=true]:translate-x-0'
        )}
        data-open={!collapsed}
      >
        {/* Logo */}
        <div className={cn(
          'flex items-center h-16 px-4 border-b border-sidebar-border',
          collapsed ? 'justify-center' : 'justify-between'
        )}>
          {!collapsed && (
            <Link href="/dashboard" className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary flex items-center justify-center">
                <Briefcase className="w-4 h-4 text-primary-foreground" />
              </div>
              <span className="font-semibold text-lg text-sidebar-foreground">JobTrack</span>
            </Link>
          )}
          {collapsed && (
            <div className="w-8 h-8 rounded-lg bg-primary flex items-center justify-center">
              <Briefcase className="w-4 h-4 text-primary-foreground" />
            </div>
          )}
          <Button
            variant="ghost"
            size="icon"
            className={cn('h-8 w-8 hidden md:flex', collapsed && 'rotate-180')}
            onClick={() => setCollapsed(!collapsed)}
          >
            <ChevronLeft className="h-4 w-4" />
          </Button>
        </div>

        {/* Navigation */}
        <nav className="flex-1 p-3 space-y-1 overflow-y-auto">
          {navItems.map((item) => {
            const isActive = pathname === item.href || pathname.startsWith(item.href + '/');
            return (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  'flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors',
                  isActive
                    ? 'bg-sidebar-accent text-sidebar-primary'
                    : 'text-sidebar-foreground/70 hover:bg-sidebar-accent/50 hover:text-sidebar-foreground',
                  collapsed && 'justify-center px-2'
                )}
                title={collapsed ? item.label : undefined}
              >
                <item.icon className="h-5 w-5 flex-shrink-0" />
                {!collapsed && <span>{item.label}</span>}
              </Link>
            );
          })}
        </nav>

        {/* User section */}
        <div className={cn(
          'p-3 border-t border-sidebar-border',
          collapsed && 'flex flex-col items-center'
        )}>
          {!collapsed && user && (
            <div className="px-3 py-2 mb-2 flex items-center justify-between">
              <p className="text-sm font-medium text-sidebar-foreground truncate">
                {user.email}
              </p>
              <ThemeToggle />
            </div>
          )}
          {collapsed && (
            <div className="mb-2">
              <ThemeToggle />
            </div>
          )}
          <Button
            variant="ghost"
            onClick={handleLogout}
            className={cn(
              'w-full justify-start gap-3 text-sidebar-foreground/70 hover:text-destructive hover:bg-destructive/10',
              collapsed && 'justify-center px-2'
            )}
          >
            <LogOut className="h-5 w-5 flex-shrink-0" />
            {!collapsed && <span>Logout</span>}
          </Button>
        </div>
      </aside>

      {/* Mobile overlay */}
      {!collapsed && (
        <div
          className="fixed inset-0 bg-black/50 z-30 md:hidden"
          onClick={() => setCollapsed(true)}
        />
      )}
    </>
  );
}
