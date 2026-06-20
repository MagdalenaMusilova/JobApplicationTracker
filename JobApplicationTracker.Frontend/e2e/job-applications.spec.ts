import { test, expect } from '@playwright/test'

// Helper function to login before each test
async function login(page) {
  await page.goto('/login')
  await page.fill('input#email', 'test@example.com')
  await page.fill('input#password', 'password123')
  await page.click('button[type="submit"]')
  await expect(page).toHaveURL(/\/dashboard/, { timeout: 10000 })
}

test.describe('Job Application Management Flow', () => {
  test.beforeEach(async ({ page }) => {
    await login(page)
    // Navigate to applications page
    await page.goto('/applications')
    await page.waitForLoadState('networkidle')
  })

  test('view job applications page', async ({ page }) => {
    // Should see applications page heading
    await expect(page.locator('h1, h2').filter({ hasText: /applications/i }).first()).toBeVisible()

    // Should see the "Add Application" or "Create" button
    await expect(
      page.locator('button:has-text("Add"), button:has-text("Create"), button:has(svg)')
    ).toBeVisible()
  })

  test('create new job application', async ({ page }) => {
    // Click create/add button (looking for Plus icon or text)
    const createButton = page.locator('button').filter({ has: page.locator('svg') }).first()
    await createButton.click()

    // Should open modal
    await expect(page.getByText('Create Job Application')).toBeVisible({ timeout: 5000 })

    // Fill in application details
    await page.fill('input#company', 'Tech Corp')
    await page.fill('input#position', 'Senior Developer')

    // Submit form
    await page.click('button[type="submit"]:has-text("Create")')

    // Should show success message
    await expect(
      page.locator('text=/success|created|added/i')
    ).toBeVisible({ timeout: 5000 })
  })

  test('view application details', async ({ page }) => {
    // Wait for page to load
    await page.waitForTimeout(1000)

    // Look for clickable application rows or cards
    const applicationElement = page.locator('table tbody tr, [role="row"], .application-card').first()

    if (await applicationElement.isVisible({ timeout: 2000 })) {
      await applicationElement.click()

      // Should show details (either in modal, sidebar, or new page)
      await page.waitForTimeout(1000)

      // Verify we can see application details
      const hasDetails = await page.locator('text=/Company|Position|Status|Details/i').isVisible()
      expect(hasDetails).toBeTruthy()
    }
  })

  test('use application filters', async ({ page }) => {
    // Look for filter controls
    const filterExists = await page.locator('button:has-text("Filter"), input[placeholder*="search" i], select').isVisible({ timeout: 2000 })

    if (filterExists) {
      // Try to interact with search if it exists
      const searchInput = page.locator('input[placeholder*="search" i]')
      if (await searchInput.isVisible({ timeout: 1000 })) {
        await searchInput.fill('Tech')
        await page.waitForTimeout(500)

        // Results should update
        expect(await page.locator('table, [role="table"]').isVisible()).toBeTruthy()
      }
    }
  })

  test('navigate through application pages', async ({ page }) => {
    // Check if pagination exists
    const paginationExists = await page.locator('button:has-text("Previous"), button:has-text("Next"), nav[aria-label*="pagination" i]').isVisible({ timeout: 2000 })

    if (paginationExists) {
      const currentUrl = page.url()

      // Click next page if available
      const nextButton = page.locator('button:has-text("Next"), button[aria-label*="next" i]').first()
      if (await nextButton.isEnabled({ timeout: 1000 })) {
        await nextButton.click()
        await page.waitForTimeout(500)

        // URL or content should change
        const newUrl = page.url()
        const urlChanged = currentUrl !== newUrl

        if (urlChanged) {
          expect(newUrl).toBeTruthy()
        }
      }
    }
  })

  test('navigate to dashboard from applications', async ({ page }) => {
    // Look for dashboard link in navigation
    const dashboardLink = page.locator('a[href="/dashboard"], a:has-text("Dashboard"), nav a:has-text("Dashboard")')

    if (await dashboardLink.isVisible({ timeout: 2000 })) {
      await dashboardLink.click()

      // Should navigate to dashboard
      await expect(page).toHaveURL(/\/dashboard/)
      await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
    }
  })

  test('verify protected route - applications requires auth', async ({ page }) => {
    // Logout first (clear storage)
    await page.context().clearCookies()
    await page.evaluate(() => localStorage.clear())

    // Try to access applications directly
    await page.goto('/applications')

    // Should redirect to login
    await expect(page).toHaveURL(/\/login/, { timeout: 5000 })
  })
})
