import { test, expect } from '@playwright/test'

test.describe('Authentication Flow', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/')
  })

  test('should redirect to login from home page when not authenticated', async ({ page }) => {
    // Home page should redirect to login
    await expect(page).toHaveURL(/\/login/)

    // Should see login form
    await expect(page.getByText('Welcome back')).toBeVisible()
    await expect(page.getByRole('button', { name: /sign in/i })).toBeVisible()
  })

  test('complete registration flow', async ({ page }) => {
    // Navigate to registration page
    await page.goto('/register')

    // Should see registration form
    await expect(page.getByText('Create an account')).toBeVisible()

    // Fill in registration form - simpler version matching actual form
    await page.fill('input[name="email"]', 'john.doe@example.com')
    await page.fill('input[name="password"]', 'SecurePass123')
    await page.fill('input[name="confirmPassword"]', 'SecurePass123')

    // Submit registration
    await page.click('button[type="submit"]')

    // Should redirect to dashboard after successful registration
    await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })

    // Verify user is logged in by checking for dashboard content
    await expect(page.getByText('Dashboard')).toBeVisible({ timeout: 10000 })
  })

  test('complete login flow', async ({ page }) => {
    // Should already be on login page from beforeEach redirect
    await page.goto('/login')

    // Fill in login credentials (actual form field IDs)
    await page.fill('input#email', 'test@example.com')
    await page.fill('input#password', 'password123')

    // Submit login form (actual button text)
    await page.click('button[type="submit"]:has-text("Sign in")')

    // Should redirect to dashboard
    await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })

    // Verify successful login
    await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
  })

  test('show error for invalid login', async ({ page }) => {
    await page.goto('/login')

    // Fill in invalid credentials
    await page.fill('input#email', 'invalid@example.com')
    await page.fill('input#password', 'wrongpassword')

    // Submit form
    await page.click('button[type="submit"]')

    // Should show error toast message
    await expect(
      page.locator('text=/Invalid credentials/i')
    ).toBeVisible({ timeout: 5000 })

    // Should stay on login page
    await expect(page).toHaveURL(/\/login/)
  })

  test('password validation during registration', async ({ page }) => {
    await page.goto('/register')

    // Fill form with weak password
    await page.fill('input[name="email"]', 'john@example.com')
    await page.fill('input[name="password"]', '123')
    await page.fill('input[name="confirmPassword"]', '456')

    // Submit form
    await page.click('button[type="submit"]')

    // Should show validation error for password requirements
    await expect(
      page.locator('text=/at least 6 characters|must include|do not match/i')
    ).toBeVisible({ timeout: 5000 })
  })

  test('toggle password visibility', async ({ page }) => {
    await page.goto('/login')

    const passwordInput = page.locator('input#password')

    // Password should be hidden initially
    await expect(passwordInput).toHaveAttribute('type', 'password')

    // Click the eye icon to show password
    await page.click('button:has(svg)')

    // Password should now be visible
    await expect(passwordInput).toHaveAttribute('type', 'text')
  })

  test('navigate between login and register', async ({ page }) => {
    await page.goto('/login')

    // Click "Sign up" link
    await page.click('a:has-text("Sign up")')

    // Should navigate to register page
    await expect(page).toHaveURL(/\/register/)
    await expect(page.getByText('Create an account')).toBeVisible()

    // Click "Sign in" link to go back
    await page.click('a:has-text("Sign in")')

    // Should be back on login page
    await expect(page).toHaveURL(/\/login/)
    await expect(page.getByText('Welcome back')).toBeVisible()
  })

  test('persist authentication after page reload', async ({ page }) => {
    // Login first
    await page.goto('/login')
    await page.fill('input#email', 'test@example.com')
    await page.fill('input#password', 'password123')
    await page.click('button[type="submit"]')

    // Wait for dashboard
    await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })

    // Reload page
    await page.reload()

    // Should still be on dashboard (authentication persisted)
    await expect(page).toHaveURL(/\/dashboard/)
    await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
  })

  test('redirect to dashboard when accessing login while authenticated', async ({ page }) => {
    // Login first
    await page.goto('/login')
    await page.fill('input#email', 'test@example.com')
    await page.fill('input#password', 'password123')
    await page.click('button[type="submit"]')

    await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })

    // Try to access login page while authenticated
    await page.goto('/login')

    // Should redirect back to dashboard
    await expect(page).toHaveURL(/\/dashboard/)
  })
})
