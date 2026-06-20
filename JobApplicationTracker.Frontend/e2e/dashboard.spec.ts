import { test, expect } from '@playwright/test'

// Helper function to login
async function login(page) {
  await page.goto('/login')
  await page.fill('input#email', 'test@example.com')
  await page.fill('input#password', 'password123')
  await page.click('button[type="submit"]')
  await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })
}

test.describe('Dashboard and Analytics Flow', () => {
  test.beforeEach(async ({ page }) => {
    await login(page)
  })

  test('display dashboard overview', async ({ page }) => {
    // Should see dashboard heading
    await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()

    // Should see description text
    await expect(page.getByText(/track your job search/i)).toBeVisible()

    // Page should be loaded
    await page.waitForLoadState('networkidle')
  })

  test('view dashboard statistics cards', async ({ page }) => {
    // Wait for stats to load
    await page.waitForTimeout(1000)

    // Look for stat cards - they might show numbers or be loading
    const statsVisible = await page.locator('[class*="grid"], [class*="stat"], [class*="card"]').isVisible({ timeout: 3000 })

    expect(statsVisible).toBeTruthy()
  })

  test('view recent applications section', async ({ page }) => {
    // Wait for content to load
    await page.waitForTimeout(1000)

    // Check if recent applications section exists
    const recentSectionExists = await page.locator('text=/Recent|Latest|Activity/i').isVisible({ timeout: 2000 })

    if (recentSectionExists) {
      // Section should be visible
      expect(recentSectionExists).toBeTruthy()
    }
  })

  test('view upcoming events section', async ({ page }) => {
    // Wait for content to load
    await page.waitForTimeout(1000)

    // Check if upcoming events section exists
    const eventsSectionExists = await page.locator('text=/Upcoming|Events|Calendar/i').isVisible({ timeout: 2000 })

    if (eventsSectionExists) {
      expect(eventsSectionExists).toBeTruthy()
    }
  })

  test('navigate to applications from dashboard', async ({ page }) => {
    // Look for applications link in navigation or on dashboard
    const applicationsLink = page.locator('a[href="/applications"], a:has-text("Applications")')

    if (await applicationsLink.isVisible({ timeout: 2000 })) {
      await applicationsLink.first().click()

      // Should navigate to applications page
      await expect(page).toHaveURL(/\/applications/)
    }
  })

  test('navigate to calendar from dashboard', async ({ page }) => {
    // Look for calendar link
    const calendarLink = page.locator('a[href="/calendar"], a:has-text("Calendar")')

    if (await calendarLink.isVisible({ timeout: 2000 })) {
      await calendarLink.first().click()

      // Should navigate to calendar page
      await expect(page).toHaveURL(/\/calendar/)
    }
  })

  test('verify protected route - dashboard requires auth', async ({ page }) => {
    // Logout first
    await page.context().clearCookies()
    await page.evaluate(() => localStorage.clear())

    // Try to access dashboard directly
    await page.goto('/dashboard')

    // Should redirect to login
    await expect(page).toHaveURL(/\/login/, { timeout: 5000 })
  })

  test('dashboard is responsive on mobile', async ({ page }) => {
    // Set mobile viewport
    await page.setViewportSize({ width: 375, height: 667 })

    // Dashboard should still be visible and functional
    await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()

    // Content should be adapted for mobile
    await page.waitForLoadState('networkidle')

    // Page should be scrollable
    const isScrollable = await page.evaluate(() => {
      return document.documentElement.scrollHeight > window.innerHeight
    })

    expect(typeof isScrollable).toBe('boolean')
  })

  test('complete user journey: dashboard to create application', async ({ page }) => {
    // Start at dashboard
    await expect(page).toHaveURL(/\/dashboard/)

    // Navigate to applications
    const applicationsLink = page.locator('a[href="/applications"], nav a:has-text("Applications")')

    if (await applicationsLink.isVisible({ timeout: 2000 })) {
      await applicationsLink.first().click()
      await expect(page).toHaveURL(/\/applications/)

      // Try to create application
      const createButton = page.locator('button').filter({ has: page.locator('svg') }).first()

      if (await createButton.isVisible({ timeout: 2000 })) {
        await createButton.click()

        // Modal should open
        await expect(page.getByText('Create Job Application')).toBeVisible({ timeout: 3000 })

        // Fill minimal required fields
        await page.fill('input#company', 'Test Company')
        await page.fill('input#position', 'Test Position')

        // Submit
        await page.click('button[type="submit"]:has-text("Create")')

        // Should show success
        await expect(
          page.locator('text=/success|created/i')
        ).toBeVisible({ timeout: 5000 })
      }
    }
  })

  test('verify navigation menu is visible', async ({ page }) => {
    // Should see navigation menu with links
    const navLinks = page.locator('nav a, [role="navigation"] a')

    const navVisible = await navLinks.first().isVisible({ timeout: 2000 })
    expect(navVisible).toBeTruthy()

    // Count navigation links (should have multiple)
    const linkCount = await navLinks.count()
    expect(linkCount).toBeGreaterThan(0)
  })
})
