# Validation Testing Checklist ✅

## Client-Side Validation Tests

### Login Page (/Account/Login)

- [ ] Empty username shows error immediately
- [ ] Empty password shows error immediately
- [ ] Invalid email format shows error
- [ ] Valid input allows form submission

### Registration Page (/Account/Register)

- [ ] All required fields validated
- [ ] Email format validation works
- [ ] Password minimum length (6 chars) enforced
- [ ] Password confirmation match validated
- [ ] Phone number format validated
- [ ] Employee ID required

### Course Creation (/AdminManagement/Courses/Create)

- [ ] Course name required
- [ ] Course code format (CS101 pattern) validated
- [ ] Credit hours range (1-6) enforced
- [ ] String length limits enforced

### Student Creation (/AdminManagement/Students/Create)

- [ ] Email format validated
- [ ] Full name required
- [ ] Roll number range (1-999999) enforced
- [ ] Section selection required

## Server-Side Validation Tests

### Test Invalid Data Reaches Server

1. **Disable JavaScript in browser:**
   - Open DevTools (F12)
   - Settings → Debugger → Disable JavaScript
2. **Try submitting invalid forms:**
   - [ ] Empty login form rejected by server
   - [ ] Invalid email rejected by server
   - [ ] Password mismatch rejected by server
   - [ ] Out-of-range values rejected by server

### Test ModelState.IsValid

- [ ] Check controller returns to view when ModelState.IsValid = false
- [ ] Error messages displayed from server-side validation

## Integration Tests

### End-to-End Flow

1. **Registration Flow:**

   - [ ] Invalid registration blocked
   - [ ] Valid registration succeeds
   - [ ] Account pending approval message shown

2. **Login Flow:**

   - [ ] Invalid credentials rejected
   - [ ] Valid credentials accepted
   - [ ] Redirect to correct dashboard by role

3. **CRUD Operations:**
   - [ ] Create with invalid data blocked
   - [ ] Edit with invalid data blocked
   - [ ] Validation works consistently across all forms

## Visual Validation Tests

### UI Elements

- [ ] Error messages appear in red text
- [ ] Error messages positioned below input fields
- [ ] Form fields highlighted when invalid
- [ ] Success state shows no errors
- [ ] Validation summary displayed when present

## Browser Compatibility Tests

Test in multiple browsers:

- [ ] Chrome/Edge (Chromium)
- [ ] Firefox
- [ ] Safari (if available)
- [ ] Mobile browsers (responsive validation)

## Performance Tests

- [ ] Validation triggers without page refresh
- [ ] No console errors when validation runs
- [ ] Form submission blocked instantly (client-side)
- [ ] Server validation adds minimal delay
- [ ] Validation scripts load properly

## Specific Validation Rules

### Email Validation

- [ ] `test@example.com` ✅ Valid
- [ ] `invalidemail` ❌ Invalid
- [ ] `test@` ❌ Invalid
- [ ] `@example.com` ❌ Invalid

### Password Validation

- [ ] `Test123` ✅ Valid (6+ chars, has uppercase)
- [ ] `test` ❌ Invalid (too short, no uppercase)
- [ ] `TESTTEST` ✅ Valid (has uppercase)
- [ ] `Test@123` ✅ Valid

### Course Code Validation

- [ ] `CS101` ✅ Valid
- [ ] `MATH1001` ✅ Valid
- [ ] `cs101` ❌ Invalid (lowercase)
- [ ] `C1` ❌ Invalid (too short)
- [ ] `COURSE101` ❌ Invalid (too many letters)

### Phone Validation

- [ ] `+1234567890` ✅ Valid
- [ ] `123-456-7890` ✅ Valid
- [ ] `abcdefg` ❌ Invalid

### Roll Number Validation

- [ ] `1` ✅ Valid
- [ ] `12345` ✅ Valid
- [ ] `0` ❌ Invalid
- [ ] `-5` ❌ Invalid
- [ ] `9999999` ❌ Invalid (too large)

## Common Issues to Check

- [ ] Validation scripts loaded on all forms
- [ ] `@section Scripts` present in all form views
- [ ] `_ValidationScriptsPartial` included correctly
- [ ] Anti-forgery tokens present in forms
- [ ] ModelState checked in POST actions
- [ ] Error messages display property names correctly

## Testing Tips

1. **Quick Test Method:**

   ```
   Right-click input field → Inspect →
   Remove 'required' or other HTML5 attributes →
   Submit form → Server validation should still catch it
   ```

2. **Console Testing:**

   ```javascript
   // Check if validation is active
   $("form").validate().numberOfInvalids();
   // Should return number of invalid fields
   ```

3. **Network Tab Testing:**
   - Invalid forms should NOT send HTTP requests
   - Valid forms should send POST requests
   - Check response codes (200 OK or 400 Bad Request)

## Expected Results Summary

✅ **Client-Side:** Instant feedback, no server round-trip for basic errors
✅ **Server-Side:** Safety net catches everything client-side missed
✅ **User Experience:** Clear error messages, intuitive validation
✅ **Security:** Anti-forgery tokens, proper ModelState checks
